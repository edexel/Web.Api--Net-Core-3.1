using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NICEAPI.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext Context { get; set; }
        private readonly IMapper Mapper;
        public UnitOfWork(DbContext context)
        {
            Context = context;
      
        }
        public IQueryable<TModel> Select<TModel>(Func<TModel, bool> where = null, params string[] navigationProperties) where TModel : class
        {
            IQueryable<TModel> dbQuery = Context.Set<TModel>();

            //Apply eager loading
            foreach (string navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<TModel>(navigationProperty);

            List<TModel> list;
            if (where == null)
            {
                list = dbQuery
               .AsNoTracking()
               .ToList<TModel>();
            }
            else
            {
                list = dbQuery
               .AsNoTracking()
               .Where(where)
               .ToList<TModel>();
            }
            return list.AsQueryable();
        }

        public (IQueryable<TModel> data, int totalRows) SelectPaged<TModel>(int page, int rows, Func<TModel, object> orderBy, System.Linq.Expressions.Expression<Func<TModel, bool>> where = null, bool OrderByDesc = true, params string[] navigationProperties) where TModel : class
        {
            IQueryable<TModel> dbQuery = Context.Set<TModel>();
            //Apply eager loading
            foreach (string navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<TModel>(navigationProperty);

            //obtiene el total de item de la tabla
            int total = 0, skip = 0; ;
            total = where == null ? dbQuery.Count() : dbQuery.Where(where).Count();

            //Paginado
            if (total != 0) { 
            rows = rows == 0 ? total : rows;
            skip = rows * (page - 1);
            }

          
            bool canPage = skip < total;
            if (!canPage) // do what you wish if you can page no further
            {
                //Pongo la ultima pagina
                if(total != 0)
                    page = total / rows;//Ultima pagina

                skip = rows * (page == 0 ? 1 : page - 1);
            }

            List<TModel> list;
            if (where == null)
            {
                list = dbQuery
               .AsNoTracking()
              // .OrderByDescending(orderBy)
               .Skip(skip)
               .Take(rows)
               .ToList<TModel>();
            }
            else
            {
                if (OrderByDesc)
                {
                     list = dbQuery
                     .Where(where)
                     .OrderByDescending(orderBy).Skip(skip)
                     .Take(rows)
                     .ToList<TModel>();
                }
                else
                {
                    list = dbQuery
                  .Where(where)
                  .OrderBy(orderBy).Skip(skip)
                  .Take(rows)
                  .ToList<TModel>();
                }
              
            }

            //total = list.Count();
            //List<TModel> views = new List<TModel>();
            //Mapper.Map(list, views);
            //var views = Mapper.Map<List<TModel>>(list);
            return (list.AsQueryable(), total);
        }

        public virtual TModel GetSingle<TModel>(Func<TModel, bool> where = null, params string[] navigationProperties) where TModel : class
        {
            TModel item = default(TModel);

            if (where != null)
            {
                IQueryable<TModel> dbQuery = Context.Set<TModel>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<TModel>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(where); //Apply where clause
            }

            return item;
        }

        //public TDestination To<TDestination>(Object source)
        //{
        //    return Mapper.Map<TDestination>(source);
        //}

        public virtual void InsertRange<TModel>(params TModel[] items) where TModel : class
        {
            {
                foreach (TModel item in items)
                {
                    Context.Entry(item).State = EntityState.Added;
                }
            }
        }

        public virtual void Insert<TModel>(TModel model) where TModel : class
        {
            {
                Context.Entry(model).State = EntityState.Added;

            }
        }

        public virtual void Update<TModel>(params TModel[] items) where TModel : class
        {
            {
                foreach (TModel item in items)
                {
                    Context.Entry(item).State = EntityState.Modified;
                }
            }
        }

        public void Update<TModel>(TModel item) where TModel : class
        {
            var state = Context.Entry(item).State;

            if (state != EntityState.Modified && state != EntityState.Unchanged)
            {
                Context.Entry(item).State = EntityState.Modified;
            }

            //Context.Entry(item).Property(property => property.CreationDate).IsModified = false;
        }

        public void DeleteRange<TModel>(TModel[] items) where TModel : class
        {
            for (int i = 0; i < items.Length; i++)
            {
                Context.Entry(items[i]).State = EntityState.Deleted;
            }
            //Context.Set<TModel>().RemoveRange(models);
        }

        public void Delete<TModel>(TModel item) where TModel : class
        {
            Context.Entry(item).State = EntityState.Deleted;
            //Context.Set<TModel>().Remove(model);
        }

        public void Commit()
        {
            try
            {
                Context.SaveChanges();
            }
            //catch (DbEntityValidationException ex)
            //{
            //    string errors = "";
            //    foreach (var eve in ex.EntityValidationErrors)
            //    {
            //        errors = $"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:" + Environment.NewLine;
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            errors += $"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"" + Environment.NewLine;
            //        }
            //    }
            //    //throw;

            //    throw new Exception(errors, ex);
            //}
            catch (Exception ex)
            {
                DetachAllEntities();
                throw new Exception("UnitOfWorkException |"+ex.ToString(),ex);
            }
        }

        //public static string GetAllExceptionTree(Exception exception)
        //{
        //    if (exception is DbEntityValidationException)
        //    {
        //        StringBuilder validationErrors = new StringBuilder();
        //        DbEntityValidationException ex = exception as DbEntityValidationException;
        //        foreach (var error in ex.EntityValidationErrors)
        //            foreach (var err in error.ValidationErrors)
        //                validationErrors.Append($"{err.ErrorMessage}<br/>");

        //        return validationErrors.ToString();
        //    }

        //    StringBuilder sb = new StringBuilder();
        //    do
        //    {
        //        if (sb.Length != 0)
        //            sb.Append("...");
        //        sb.Append(exception.Message);
        //        exception = exception.InnerException;
        //    } while (exception != null);

        //    return sb.ToString();
        //}


        public void Detach<TModel>(TModel model) where TModel : class
        {
            Context.Entry(model).State = EntityState.Detached;
       
        }

        public void Dispose()
        {
            Context.Dispose();
        }


        //public (List<TModel> data, int totalRows) BuildPagination<TModel>(string table, string id, string where, int row, int page) where TModel : class
        //{
        //     var where2 = where.Replace("'", "°");
        //    where2 = where2.Replace(">","@MayorQue");
        //    where2 = where2.Replace("<", "@MenorQue");
        //    Context.Database.CommandTimeout = 11180;
        //    var pagination = Context.Database.SqlQuery<TModel>("exec [Common].[spxPaginado] '" + table + "','" + id + "','<CONDICION PARAMETRO=\"" + where2 + "\"/>'," + row + "," + page).ToList();
        //    return (pagination, pagination.Count());
        //}
        //public void DeleteTable<TModel>(string table) where TModel : class
        //{
        //    var val = Context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Event].[" + table + "]");
        //}

        public void DetachAllEntities()
        {
            var changedEntriesCopy = Context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
         
    }
}
