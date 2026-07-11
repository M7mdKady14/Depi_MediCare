using Clinic.Core.Common;
using Clinic.Core.Enums;
using Clinic.Infrastructure.Context;
using Clinic.Infrastructure.Contracts;
using Clinic.Infrastructure.Exceptions;
using Clinic.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Infrastructure.Repositories
{
    public class TableRepository <T> : IGenericRepository <T> where T : BaseEntity
    {
        private readonly ClinicContext ctx;
        private readonly ILogger<TableRepository<T>> _logger;
        private DbSet<T> _dbSet;

        public TableRepository(ClinicContext context , ILogger<TableRepository<T>> logger)
        {
            ctx = context;
            _logger = logger;
            _dbSet = ctx.Set<T>();
        }

        public async Task<List<T>> GetAll()
        {
            try
            {
                return await _dbSet.Where(a => a.CurrentState == CurrentState.Active).OrderByDescending(a => a.CreatedDate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }
        }

        public async Task<T> GetById(Guid Id)
        {
            try
            {
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(a => a.CurrentState == CurrentState.Active && a.Id == Id);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Not Found", _logger);
            }
        }

        public async Task<T> GetByIdWithTracking(Guid Id)
        {
            try
            {
                return await _dbSet.FirstOrDefaultAsync(a => a.CurrentState == CurrentState.Active && a.Id == Id);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Not Found", _logger);
            }
        }

        public async Task<(bool, Guid)> Add(T entity)
        {
            try
            {
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                    entity.CreatedDate = DateTime.UtcNow;
                    entity.CurrentState = CurrentState.Active;

                    await _dbSet.AddAsync(entity);

                    return (true , entity.Id);
                }

                return (false , Guid.Empty);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Adding Fail", _logger);
            }
        }

        public async Task<bool> Update(T entity)
        {
            try
            {
                if (entity.Id != Guid.Empty)
                {
                    var data = await GetByIdWithTracking(entity.Id);

                    if (data == null)
                    {
                        return false;
                    }

                    entity.UpdatedDate = DateTime.UtcNow;

                    ctx.Entry(data).CurrentValues.SetValues(entity);

                    // منع تغيير الحالة
                    ctx.Entry(data).Property(x => x.CurrentState).IsModified = false;
                    ctx.Entry(data).Property(x => x.CreatedDate).IsModified = false;
                    ctx.Entry(data).Property(x => x.CreatedBy).IsModified = false;

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Updating Fail", _logger);
            }
        }

        public async Task<bool> ChangeStatus(Guid Id , Guid UserId , CurrentState status = CurrentState.Deleted)
        {
            try
            {
                var data = await GetByIdWithTracking(Id);

                if (data == null)
                {
                    return false;
                }

                data.CurrentState = status;
                data.UpdatedDate = DateTime.UtcNow;
                data.UpdatedBy = UserId;

                return true;
            }            
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Changing Status Fail", _logger);
            }
        }

        public async Task<bool> Delete(Guid Id)
        {
            try
            {
                var data = await GetByIdWithTracking(Id);

                if (data == null)
                {
                    return false;
                }
                
                _dbSet.Remove(data);

                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Deleting Fail", _logger);
            }
        }

        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            try
            {
                return await _dbSet.FirstOrDefaultAsync(filter);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Not Found", _logger);
            }
        }

        public async Task<List<T>> GetList(Expression<Func<T, bool>> filter)
        {
            try
            {
                return await _dbSet.Where(filter).OrderByDescending(a => a.CreatedDate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }
        }

        public async Task<PageResult<TResult>> GetPageResult<TResult>(
            int PageNumber, 
            int PageSize, 
            Expression<Func<T, bool>>? filter = null, 
            Expression<Func<T, TResult>>? selectors = null, 
            Expression<Func<T, object>>? orderby = null, 
            bool isDesending = false, 
            params Expression<Func<T, object>>[] includers
        )
        {
            try
            {
                IQueryable<T> query = _dbSet.AsQueryable();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                foreach (var include in includers)
                {
                    query = query.Include(include);
                }

                int totalCount = await query.CountAsync();

                if (orderby != null)
                {
                    query = (isDesending) ? query.OrderByDescending(orderby) : query.OrderBy(orderby);
                }

                query = query.Skip((PageNumber - 1) * PageSize).Take(PageSize);

                var items = (selectors != null) ? await query.Select(selectors).ToListAsync() : await query.Cast<TResult>().ToListAsync(); 

                return new PageResult<TResult>
                {
                    Items = items,
                    PageSize = PageSize,
                    TotalCount = totalCount,
                    PageNumber = PageNumber,
                    TotalPages = (int) Math.Ceiling(totalCount / (double) PageSize),
                };

            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }
        }

        public async Task<List<TResult>> GetResult<TResult>(
            Expression<Func<T, bool>>? filter = null, 
            Expression<Func<T, TResult>>? selectors = null, 
            Expression<Func<T, object>>? orderby = null, 
            bool isDesending = false, 
            params Expression<Func<T, object>>[] includers
        )
        {
            try
            {
                IQueryable<T> query = _dbSet.AsQueryable();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                foreach (var include in includers)
                {
                    query = query.Include(include);
                }

                if (orderby != null)
                {
                    query = (isDesending) ? query.OrderByDescending(orderby) : query.OrderBy(orderby);
                }

                if (selectors != null)
                {
                    return await query.Select(selectors).ToListAsync();
                }

                return await query.Cast<TResult>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }
        }

        public async Task<TResult> GetResultForOne<TResult>(
            Expression<Func<T, bool>>? filter = null, 
            Expression<Func<T, TResult>>? selectors = null, 
            params Expression<Func<T, object>>[] includers
        )
        {
            try
            {
                IQueryable<T> query = _dbSet.AsQueryable();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                foreach (var include in includers)
                {
                    query = query.Include(include);
                }

                if (selectors != null)
                {
                    return await query.Select(selectors).FirstOrDefaultAsync();
                }

                return await query.Cast<TResult>().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Not Found", _logger);
            }
        }

    }
}
