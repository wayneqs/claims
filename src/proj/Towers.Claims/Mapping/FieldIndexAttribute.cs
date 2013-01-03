using System;

namespace Towers.Claims.Mapping
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FieldIndexAttribute : Attribute
    {
        public FieldIndexAttribute(int index)
        {
            Index = index;
        }

        public int Index;
    }
}