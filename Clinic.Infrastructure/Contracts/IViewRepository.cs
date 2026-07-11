using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Clinic.Infrastructure.Models;

namespace Clinic.Infrastructure.Contracts
{
    public interface IViewRepository <T> where T : class
    {
        public Task<List<T>> GetAll();
        public Task<T> GetById(Guid Id);
        public Task<List<T>> GetList(Expression<Func<T, bool>> filter);
        public Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter);

        public Task<List<TResult>> GetResult<TResult>(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, TResult>>? selectors = null,
            Expression<Func<T, object>>? orderby = null,
            bool isDesending = false,
            params Expression<Func<T, object>>[] includers
        );

        public Task<PageResult<TResult>> GetPageResult<TResult>(
            int PageNumber,
            int PageSize,
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, TResult>>? selectors = null,
            Expression<Func<T, object>>? orderby = null,
            bool isDesending = false,
            params Expression<Func<T, object>>[] includers
        );

        public Task<TResult> GetResultForOne<TResult>(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, TResult>>? selectors = null,
            params Expression<Func<T, object>>[] includers
        );
    }
}
