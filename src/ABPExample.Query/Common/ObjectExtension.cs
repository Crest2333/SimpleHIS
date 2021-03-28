using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ABPExample.Domain.Dtos.Appointment;

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

    }
}
