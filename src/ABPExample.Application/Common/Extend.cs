using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace ABPExample.Application.Common
{
    public static class Extend
    {

        /// <summary>
        ///     List To DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            var dt = new DataTable(typeof(T).Name);
            var props = typeof(T).GetProperties(bindingAttr: System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var item in props)
            {
                var t = GetCoreType(item.PropertyType);
                dt.Columns.Add(item.Name, t);
            }

            foreach (var item in list)
            {
                var obj = new object[props.Length];
                for (var index = 0; index < props.Length; index++)
                {
                    obj[index] = props[index].GetValue(item);
                }
                dt.Rows.Add(obj);
            }
            return dt;
        }

        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
               else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }

        }

        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
    }
}
