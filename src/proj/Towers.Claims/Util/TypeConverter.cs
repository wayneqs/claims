using System;
using System.ComponentModel;

namespace Towers.Claims.Util
{
    public static class TypeConverter
    {
        public static bool CanConvert<T>(object value)
        {
            return CanConvert(typeof (T), value);
        }
        public static bool CanConvert(Type type, object value)
        {
            var tc = TypeDescriptor.GetConverter(type);
            return tc.IsValid(value);
        }
        public static T Convert<T>(object value)
        {
            return (T) Convert(typeof (T), value);
        }
        public static object Convert(Type type, object value)
        {
            var tc = TypeDescriptor.GetConverter(type);
            return tc.ConvertFrom(value);
        }
    }
}
