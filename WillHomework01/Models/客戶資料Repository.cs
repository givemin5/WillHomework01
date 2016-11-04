using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace WillHomework01.Models
{
    public class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(x => x.是否已刪除 != true);
        }

        internal IQueryable<客戶資料> Search(string level, string keyword, string sortOrder, bool isAsc)
        {
            var customers = this.All().AsQueryable();

            if (!String.IsNullOrEmpty(level))
            {
                customers = customers.Where(x => x.客戶分類 == level);
            }


            if (!String.IsNullOrEmpty(keyword))
            {
                customers = customers.Where(x => x.客戶名稱.Contains(keyword));
            }

            var property = typeof(客戶資料).GetProperty(sortOrder);


            if (property != null)
            {
                if (isAsc)
                {
                    customers = customers.OrderBy(sortOrder);
                }
                else
                {
                    customers = customers.OrderByDescending(sortOrder);
                }

            }

            return customers;
        }
    }

    public interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
    public static class QueryableHelper
    {
        //public static IQueryable<TModel> OrderBy<TModel>(this IQueryable<TModel> q, string name)
        //{
        //    Type entityType = typeof(TModel);
        //    PropertyInfo p = entityType.GetProperty(name);
        //    MethodInfo m = typeof(QueryableHelper).GetMethod("OrderByProperty").MakeGenericMethod(entityType, p.PropertyType);
        //    return (IQueryable<TModel>)m.Invoke(null, new object[] { q, p });
        //}

        //public static IQueryable<TModel> OrderByDescending<TModel>(this IQueryable<TModel> q, string name)
        //{
        //    Type entityType = typeof(TModel);
        //    PropertyInfo p = entityType.GetProperty(name);
        //    MethodInfo m = typeof(QueryableHelper).GetMethod("OrderByPropertyDescending").MakeGenericMethod(entityType, p.PropertyType);
        //    return (IQueryable<TModel>)m.Invoke(null, new object[] { q, p });
        //}

        //public static IQueryable<TModel> OrderByPropertyDescending<TModel, TRet>(IQueryable<TModel> q, PropertyInfo p)
        //{
        //    ParameterExpression pe = Expression.Parameter(typeof(TModel));
        //    Expression se = Expression.Convert(Expression.Property(pe, p), typeof(object));
        //    return q.OrderByDescending(Expression.Lambda<Func<TModel, TRet>>(se, pe));
        //}

        //public static IQueryable<TModel> OrderByProperty<TModel, TRet>(IQueryable<TModel> q, PropertyInfo p)
        //{
        //    ParameterExpression pe = Expression.Parameter(typeof(TModel));
        //    Expression se = Expression.Convert(Expression.Property(pe, p), typeof(object));
        //    return q.OrderBy(Expression.Lambda<Func<TModel, TRet>>(se, pe));
        //}

        public static IOrderedQueryable<TSource> OrderBy<TSource>(
       this IQueryable<TSource> query, string propertyName)
        {
            var entityType = typeof(TSource);

            //Create x=>x.PropName
            var propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(System.Linq.Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     //Put more restriction here to ensure selecting the right overload                
                     return parameters.Count == 2;//overload that has 2 parameters
                 }).Single();
            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method
                 .MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /*Call query.OrderBy(selector), with query and selector: x=> x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            var newQuery = (IOrderedQueryable<TSource>)genericMethod
                 .Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }

        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(
       this IQueryable<TSource> query, string propertyName)
        {
            var entityType = typeof(TSource);

            //Create x=>x.PropName
            var propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(System.Linq.Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderByDescending" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     //Put more restriction here to ensure selecting the right overload                
                     return parameters.Count == 2;//overload that has 2 parameters
                 }).Single();
            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method
                 .MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /*Call query.OrderBy(selector), with query and selector: x=> x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            var newQuery = (IOrderedQueryable<TSource>)genericMethod
                 .Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }
    }
}

