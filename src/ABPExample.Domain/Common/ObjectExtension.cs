using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ABPExample.Domain.Common
{
    public static class ObjectExtension
    {
        public static string GetDescription(this object obj)
        {
            if (obj == null)
                return null;
            var type = obj.GetType();
            var fieldInfo = type.GetField(Enum.GetName(type, obj) ?? string.Empty);
            if (fieldInfo == null) return null;
            if (!Attribute.IsDefined(fieldInfo, typeof(DescriptionAttribute))) return null;
            var description =
                Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return description?.Description;
        }

    }
}
