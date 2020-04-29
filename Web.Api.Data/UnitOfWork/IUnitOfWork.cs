using System;
using System.Collections.Generic;
using System.Linq;

namespace NICEAPI.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IQueryable<TModel> Select<TModel>(Func<TModel, bool> where = null, params string[] navigationProperties) where TModel : class;

        (IQueryable<TModel> data , int totalRows) SelectPaged<TModel>(int page, int rows, Func<TModel, object> orderBy, System.Linq.Expressions.Expression<Func<TModel, bool>> where = null, bool OrderByDesc = true, params string[] navigationProperties) where TModel : class;

        //(List<TModel> data, int totalRows) BuildPagination<TModel>(string table, string id, string where, int row, int page) where TModel : class;

        TModel GetSingle<TModel>(Func<TModel, bool> where = null, params string[] navigationProperties) where TModel : class;

        //TDestination To<TDestination>(Object source);

        void InsertRange<TModel>(params TModel[] items) where TModel : class;
        void Update<TModel>(params TModel[] items) where TModel : class;
        void Insert<TModel>(TModel model) where TModel : class;
        void DeleteRange<TModel>(TModel[] items) where TModel : class;
        void Delete<TModel>(TModel model) where TModel : class;

        void Commit();
    }
}
