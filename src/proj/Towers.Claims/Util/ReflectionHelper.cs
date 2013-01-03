using System;
using System.Linq;
using System.Reflection;

namespace Towers.Claims.Util
{
    public class ReflectionHelper
    {
        public static T GetAttribute<T>(MemberInfo memberInfo) where T : Attribute
        {
            return (T)memberInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
        }

        public static T GetAttribute<T>(Type type) where T : Attribute
        {
            return (T)type.GetCustomAttributes(typeof(T), false).FirstOrDefault();
        }
    }
}