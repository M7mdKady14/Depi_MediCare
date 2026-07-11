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
    public class ViewRepository <T> : IViewRepository <T> where T : BaseEntity
    {
        private readonly ClinicContext ctx;
        private readonly ILogger<ViewRepository<T>> _logger;
        private DbSet<T> _dbSet;

        public ViewRepository(ClinicContext context, ILogger<ViewRepository<T>> logger)
        {
            ctx = context;
            _logger = logger;
            _dbSet = ctx.Set<T>();
        }

        public async Task<List<T>> GetAll()
        {
            try
            {
                return await _dbSet.AsNoTracking().Where(a => a.CurrentState == CurrentState.Active).OrderByDescending(a => a.CreatedDate).ToListAsync();
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
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(a => a.Id == Id);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Not Found", _logger);
            }
        }

        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            try
            {
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(filter);
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
                return await _dbSet.AsNoTracking().Where(filter).OrderByDescending(a => a.CreatedDate).ToListAsync();
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
                IQueryable<T> query = _dbSet.AsNoTracking().AsQueryable();

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
                    TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize),
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
                IQueryable<T> query = _dbSet.AsNoTracking().AsQueryable();

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
                IQueryable<T> query = _dbSet.AsNoTracking().AsQueryable();

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
