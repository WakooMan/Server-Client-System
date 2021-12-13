using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class JsonMessageProtocol: Protocol<JObject>
    {
        private readonly JsonSerializer _serializer;
        private readonly JsonSerializerSettings _settings;

        public JsonMessageProtocol()
        {
            _settings = new JsonSerializerSettings
            {
                Formatting= Formatting.Indented,
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

        protected override JObject Decode(byte[] Message)
            => JObject.Parse(Encoding.UTF8.GetString(Message));

        protected override byte[] EncodeBody<T>(T message)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            _serializer.Serialize(sw,message);
            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
