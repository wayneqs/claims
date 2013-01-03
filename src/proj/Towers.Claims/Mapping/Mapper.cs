using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Towers.Claims.Util;

namespace Towers.Claims.Mapping
{
    public class Mapper<TToType> where TToType : class, new()
    {
        private readonly ErrorCollector _collector;
        private readonly FieldInfo[] _expectedFields;
        private readonly Dictionary<string, int> _fieldIndexLookup = new Dictionary<string, int>();

        public Mapper(ErrorCollector collector)
        {
            _collector = collector;
            var type = typeof(TToType);
            _expectedFields = (from m in type.GetMembers().OfType<FieldInfo>()
                               orderby m.MetadataToken
                               select m).ToArray();
            foreach (var field in _expectedFields)
            {
                var attribute = ReflectionHelper.GetAttribute<FieldIndexAttribute>(field);
                _fieldIndexLookup[field.Name] = attribute == null ? -1 : attribute.Index;
            }
        }

        public TToType Map(string[] from)
        {
            if (from == null) return null;

            var instance = Activator.CreateInstance<TToType>();

            for (int i = 0; i < _expectedFields.Length; i++)
            {
                int index = i;
                if ( _fieldIndexLookup[_expectedFields[i].Name] != -1 )
                    index = _fieldIndexLookup[_expectedFields[i].Name];
                if (index >= from.Length)
                {
                    _collector.Add("Missing input data: Field '{0}' on type '{1}' is expecting data at column index {2} in the input source.", _expectedFields[i].Name, typeof(TToType).Name, index);
                    continue;
                }
                var field = _expectedFields[i];
                if (!TypeConverter.CanConvert(field.FieldType, from[index]))
                {
                    _collector.Add(string.Format("Invalid input data: source column index '{0}' containing the value '{1}' cannot be converted into {2}", index, from[index], field.FieldType.Name));
                    continue;
                }
                field.SetValue(instance, TypeConverter.Convert(field.FieldType, from[index]));
            }
            return instance;
        }
    }
}