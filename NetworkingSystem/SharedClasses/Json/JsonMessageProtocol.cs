using Newtonsoft.Json.Linq;
using System.Text;


namespace SharedClasses{
    public class JsonMessageProtocol: Protocol<JObject>
    {

        protected override JObject Decode(byte[] Message)
            => JsonSerialization.Deserialize(Encoding.UTF8.GetString(Message));

        protected override byte[] EncodeBody<T>(T message) => Encoding.UTF8.GetBytes(JsonSerialization.Serialize(message).ToString());
    }
}
