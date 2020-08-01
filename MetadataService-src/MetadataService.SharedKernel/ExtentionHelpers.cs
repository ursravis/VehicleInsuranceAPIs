using System;
using System.Collections.Generic;
using System.Text;

namespace MetadataService.SharedKernel
{
    public static class ExtentionHelpers
    {
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        public static string AddwhildChars(this string value)
        {
            return "%"+ value+"%";
        }
        public static string SafeLower(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            else return value.ToLower();
        }
    }
}
