using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Appointment;
using Microsoft.EntityFrameworkCore;

namespace ABPExample.Query.Common
{
    public static class ObjectExtension
    {
        public static string GetDescription(this Enum obj)
        {
            if (obj == null)
                return null;
            var type = typeof(object);
            var fieldInfo = type.GetField(Enum.GetName(type, obj) ?? string.Empty);
            if (fieldInfo == null) return null;
            if (!Attribute.IsDefined(fieldInfo, typeof(DescriptionAttribute))) return null;
            var description =
                Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return description?.Description;
        }


        /// <summary>
        ///    是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this string obj)
        {
           return string.IsNullOrEmpty(obj);
        }

        public static async Task<IList<T>> ToListAsync<T>(this IQueryable<T> query, int pageIndex, int pageSize) where T:class
        {
           return await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public static int ToInt(this string obj)
        {
           return Convert.ToInt32(obj);
        }
    }
}
