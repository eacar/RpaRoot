using Rpa.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rpa.Extensions
{
    public static class ObjectExtensions
    {
        public static string GetPropertyName<TPropertySource>(this Expression<Func<TPropertySource, object>> expression)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            if (memberExpression == null)
                throw new ArgumentException("Please provide a lambda expression like 'n => n.PropertyName'");

            var propertyInfo = memberExpression.Member as PropertyInfo;

            if (propertyInfo == null)
                throw new NullReferenceException("propertyInfo cannot be null");

            return propertyInfo.Name;
        }
        public static int ToColumnIndex(this Type type, string name)
        {
            var prop = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => p.Name == name);

            if (prop == null)
                return -1;

            var attr = (from row in prop.GetCustomAttributes(false)
                    where row.GetType() == typeof(ColumnIndexAttribute)
                    select row)
                .FirstOrDefault();


            return ((ColumnIndexAttribute)attr)?.Value ?? -1;
        }
        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
        public static void CopyProperty<TParent, TChild>(this TParent parent, TChild child) 
            where TParent : class
            where TChild : class
        {
            var parentProperties = parent.GetType().GetProperties();
            var childProperties = child.GetType().GetProperties();

            foreach (var parentProperty in parentProperties)
            {
                foreach (var childProperty in childProperties)
                {
                    if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
                    {
                        childProperty.SetValue(child, parentProperty.GetValue(parent));
                        break;
                    }
                }
            }
        }
        public static List<Variance> DetailedCompare<T>(this T val1, T val2)
        {
            var variances = new List<Variance>();

            var fi = val1.GetType().GetProperties();
            foreach (var f in fi)
            {
                Variance v = new Variance();
                v.Prop = f.Name;
                v.ValA = f.GetValue(val1);
                v.ValB = f.GetValue(val2);



                if (!Equals(v.ValA, v.ValB))
                {
                    if (v.ValA is IList && v.ValB is IList)
                    {
                        if (((IList)v.ValA).Count != ((IList)v.ValB).Count)
                        {
                            variances.Add(v);
                        }
                        else
                        {
                            for (int i = 0; i < ((IList)v.ValA).Count; i++)
                            {
                                var value1 = ((IList)v.ValA)[i];
                                var value2 = ((IList)v.ValB)[i];

                                if (IsSimpleType(value1.GetType()) && IsSimpleType(value2.GetType()))
                                {
                                    if (!Equals(value1, value2))
                                    {
                                        variances.Add(v);
                                        break;
                                    }
                                }
                                else
                                {
                                    var diff = value1.DetailedCompare(value2);
                                    if (diff.Any())
                                    {
                                        variances.Add(v);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                        variances.Add(v);
                }

            }
            return variances;
        }
        public static bool IsSimpleType(this Type type)
        {
            return
                type.IsPrimitive ||
                new Type[] {
                    typeof(string),
                    typeof(decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }.Contains(type) ||
                type.IsEnum ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]))
                ;
        }
        public static T OverridePropsWhenTargetHasDefaultValue<T>(this object sourceObj, T targetObj)
        {
            var sourceObjProps = sourceObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(c => c.CanWrite && c.CanRead);

            var targetObjProps = targetObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(c => c.CanWrite && c.CanRead);

            foreach (PropertyInfo targetObjProp in targetObjProps)
            {
                foreach (var sourceObjProp in sourceObjProps)
                {
                    if (sourceObjProp.Name == targetObjProp.Name
                        && sourceObjProp.PropertyType == targetObjProp.PropertyType)
                    {
                        var sourceVal = sourceObj.GetPropValue(sourceObjProp.Name);
                        var targetVal = targetObj.GetPropValue(targetObjProp.Name);
                        object targetValDefault = 
                            targetObjProp.PropertyType.IsValueType
                            ? Activator.CreateInstance(targetObjProp.PropertyType)
                            : null;

                        if (targetVal == targetValDefault || targetVal.Equals(targetValDefault))
                        {
                            targetObjProp.SetValue(targetObj, sourceVal);
                        }
                    }
                }
            }

            return targetObj;
        }
        public static object GetPropValue(this object obj, string name)
        {
            foreach (string part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }
        public static T GetPropValue<T>(this object obj, string name)
        {
            object retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }

        public static T ToObject<T>(this IDictionary<string, object> source)
            where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                someObjectType
                    .GetProperty(item.Key)
                    ?.SetValue(someObject, item.Value, null);
            }

            return someObject;
        }

        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );

        }
    }
    public class Variance
    {
        public string Prop { get; set; }
        public object ValA { get; set; }
        public object ValB { get; set; }
    }
}