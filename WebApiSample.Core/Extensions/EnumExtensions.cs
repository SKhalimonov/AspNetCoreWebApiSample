using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace WebApiSample.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            if (fieldInfo == null)
            {
                return null;
            }

            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));

            return attribute?.Description ?? value.ToString();
        }

        public static List<TEnum> GetList<TEnum>()
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new Exception("Type given TEnum must be an Enum");
            }

            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
        }
    }
}
