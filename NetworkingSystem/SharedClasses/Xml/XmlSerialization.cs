using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SharedClasses{
    public static class XmlSerialization
    {
        public static XDocument Serialize<T>(T instance) => Serialize(typeof(T),instance);

        private static XDocument Serialize(Type type, object instance)
        {
            var ms = new MemoryStream();
            var xs = new XmlSerializer(type);
            xs.Serialize(ms, instance);
            ms.Flush();
            ms.Position = 0L;
            return XDocument.Load(ms);
        }

        public static T Deserialize<T>(XDocument xml) => (T)Deserialize(typeof(T), xml);

        private static object Deserialize(Type type, XDocument xml)=> new XmlSerializer(type).Deserialize(new StringReader(xml.ToString()));
    }
}
