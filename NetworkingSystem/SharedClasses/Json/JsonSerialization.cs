using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public static class JsonSerialization
    {
        static readonly JsonSerializer _serializer;
        static readonly JsonSerializerSettings _settings;

        static JsonSerialization()
        {
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new DefaultContractResolver 
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        ProcessDictionaryKeys = false,
                    }
                }
            };
            _settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            _serializer = JsonSerializer.Create(_settings);
        }

        public static JObject Serialize(object Object) => JObject.FromObject(Object,_serializer);

        internal static object Serialize(Type type, JObject message)
        {
            throw new NotImplementedException();
        }

        public static JObject Deserialize(string json) => JObject.Parse(json);
        public static T Deserialize<T>(JObject obj) => obj.ToObject<T>();

        public static object ToObject(Type type, JObject source) => source.ToObject(type);

    }
}
