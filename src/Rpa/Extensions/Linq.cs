using Rpa.Contracts;
using Rpa.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rpa.Extensions
{
    public static class LinqExtension
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderByField"></param>
        /// <param name="sortDirectionType"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderByFieldName<T>(this IQueryable<T> source, string orderByField, SortDirectionType sortDirectionType = SortDirectionType.Asc)
        {
            string command = sortDirectionType == SortDirectionType.Desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(T);

            if (string.IsNullOrWhiteSpace(orderByField))
                throw new ArgumentException("orderByField must be given a value!");

            var property = type.GetProperty(orderByField);
            if (property == null)
            {
                return source;
            }


            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<T>(resultExpression);
        }
        public static T GetAbsoluteParent<T>(this IList<T> list, T item = default(T)) where T : ITreeViewList<T>
        {
            T result = item;

            if (result != null && result.Parent != null)
            {
                return list.GetAbsoluteParent(result.Parent);
            }

            return result;
        }
        public static IEnumerable<T> Traverse<T>(this IList<T> list) where T : ITreeViewList<T>
        {
            foreach (var root in list)
            {
                var stack = new Stack<T>();
                stack.Push(root);
                while (stack.Count > 0)
                {
                    var current = stack.Pop();
                    yield return current;
                    if (current.HasChildren)
                    {
                        foreach (var child in current.Children)
                            stack.Push(child);
                    }
                }
            }
        }
        public static IEnumerable<T> Traverse<T, TId>(this IList<T> list) where T : ITreeViewList<T, TId>
        {
            foreach (var root in list)
            {
                var stack = new Stack<T>();
                stack.Push(root);
                while (stack.Count > 0)
                {
                    var current = stack.Pop();
                    yield return current;
                    if (current.HasChildren)
                    {
                        foreach (var child in current.Children)
                            stack.Push(child);
                    }
                }
            }
        }
        public static List<T> RemoveItems<T>(this List<T> items, Func<T, bool> expression)
        {
            if (items.Any(expression))
            {
                var itemToRemove = items.First(expression);

                items.Remove(itemToRemove);

                return RemoveItems(items, expression);
            }

            return items;
        }
    }
}