using System;
using System.Runtime.Serialization;

namespace Networking.Implementation
{
    [Serializable]
    internal class ClientAlreadyHasGuidException : Exception
    {
        public ClientAlreadyHasGuidException()
        {
        }

        public ClientAlreadyHasGuidException(string message) : base(message)
        {
        }

        public ClientAlreadyHasGuidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClientAlreadyHasGuidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}