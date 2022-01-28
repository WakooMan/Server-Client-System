using System;

namespace Networking{

    [Serializable]
    public class KeepAliveResponseMessage : ReliableMessage
    {
        public Result Result { get; set; }

        public KeepAliveResponseMessage(Result result) : base(1,MessageType.Response)
        {
            Result = result;
        }

        public override string ToString()
        {
            return "KeepAliveResponseMessage";
        }
    }
}
