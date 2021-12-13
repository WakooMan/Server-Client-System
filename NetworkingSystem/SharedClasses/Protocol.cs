using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses{
    public abstract class Protocol<TMessageType>
    {
        //This is the size of an int in bytes.
        const int HEADER_SIZE = 4;
        
        //Receives a message from the stream and decodes it to the message object.
        public async Task<TMessageType> ReceiveAsync(NetworkStream stream)
        {
            int BodyLength = await ReadHeader(stream).ConfigureAwait(false);
            AssertValidMessageLength(BodyLength);
            return await ReadBody(stream,BodyLength).ConfigureAwait(false);
        }

        //Encodes the message to bytes and writes it to the stream. 
        public async Task SendAsync<T>(NetworkStream stream, T message)
        {
            var (header, body) = Encode<T>(message);
            var Data = new byte[header.Length + body.Length];
            Buffer.BlockCopy(src:header,0,dst:Data,0,header.Length);
            Buffer.BlockCopy(src:body,0,dst:Data,header.Length,body.Length);
            await stream.WriteAsync(Data,0,Data.Length);
        }

        //Reads the length of the message and returns with it.
        private async Task<int> ReadHeader(NetworkStream stream)
        {
            byte[] headerbytes = await ReadAsync(stream, HEADER_SIZE).ConfigureAwait(false);
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(headerbytes,0));
        }

        //Returns with the bytes representing the length of the message and the message itself in bytes in two different arrays.
        protected (byte[] header, byte[] body) Encode<T>(T Message)
        {
            byte[] bodybytes = EncodeBody(Message);
            byte[] headerbytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(bodybytes.Length));
            return (headerbytes, bodybytes);
        }
        //Reads the body from the stream and returns with the decoded message.
        private async Task<TMessageType> ReadBody(NetworkStream stream,int BodyLength)
        {
            byte[] BodyBytes = await ReadAsync(stream, BodyLength).ConfigureAwait(false);
            return Decode(BodyBytes);
        }

        //Reads the given amount of bytes from the stream
        private async Task<byte []> ReadAsync(NetworkStream stream, int BytesToRead)
        {
            byte[] buffer = new byte[BytesToRead];
            int BytesRead = 0;
            while (BytesRead<BytesToRead)
            {
                int BytesReceived = await stream.ReadAsync(buffer,BytesRead,(BytesToRead-BytesRead)).ConfigureAwait(false);
                if (BytesReceived == 0)
                    throw new Exception("Socket has been closed");
                BytesRead += BytesReceived;
            }
            return buffer;
        }

        //We assert if the header is valid.
        protected virtual void AssertValidMessageLength(int messageLength)
        {
            if (messageLength < 1)
                throw new ArgumentOutOfRangeException("Invalid Message Length");
        }

        //Decodes the message.
        protected abstract TMessageType Decode(byte[] Message);

        //Encodes the message.
        protected abstract byte[] EncodeBody<T>(T message);
    }
}
