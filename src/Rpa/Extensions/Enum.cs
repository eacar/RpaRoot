using Rpa.Attributes;
using Rpa.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Rpa.Extensions
{
    public static class EnumExtensions
    {
        #region Methods - Public

        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
        }
        [Obsolete]
        public static TOut ToEnumAttributeValue<TEnum, TAttribute, TOut>(this TEnum value, TOut defaultValue = default)
            where TAttribute : IEnumAttribute<TOut>
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi == null)
                return defaultValue;

            IEnumAttribute<TOut>[] attributes = (IEnumAttribute<TOut>[])fi.GetCustomAttributes(typeof(TAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Value;

            return defaultValue;
        }
        public static T ToEnum<T>(this string value, bool ignoreCase = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return default(T);

            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }
        [Obsolete]
        public static T ToEnumFromEnumString<T>(this string value, bool ignoreCase = true) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                return default(T);

            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (value == item.GetAttribute<StringValue>().Value)
                {
                    return item;
                }
            }

            return value.ToEnum<T>();
        }
        [Obsolete]
        public static string ToEnumString(this Enum value)
        {
            string output = value.ToString();
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs =
                fi.GetCustomAttributes(typeof(StringValue),
                    false) as StringValue[];
            if (attrs?.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }
        [Obsolete]
        public static HttpStatusCode ToEnumHttpStatusCode(this Enum value)
        {
            HttpStatusCode output = HttpStatusCode.OK;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            HttpStatusCodeValue[] attrs =
                fi.GetCustomAttributes(typeof(HttpStatusCodeValue),
                    false) as HttpStatusCodeValue[];
            if (attrs?.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }
        public static IList<T> ToEnumList<T>(params T[] excludes) where T : struct, Enum
        {
            if (excludes == null || !excludes.Any())
                return Enum.GetValues(typeof(T)).Cast<T>().ToList();

            return Enum.GetValues(typeof(T)).Cast<T>()
                .Where(c => excludes.All(p => !p.Equals(c)))
                .ToList();
        }
        public static T[] ToEnumArray<T>(this int[] enums) where T : struct, Enum
        {
            return enums?.Select(c => c.Convert<T>()).ToArray();
        }

        #endregion
    }
}