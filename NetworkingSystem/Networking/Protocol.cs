using LiteNetLib;
using System;

namespace Networking{
    public abstract class Protocol<TMessageType>
    {
        //This is the size of an int in bytes.
        const int HEADER_SIZE = 4;

        public TMessageType Receive(byte[] bytes)
        {
            int BodyLen = ReadHeader(bytes);
            return ReadBody(bytes, BodyLen);
        }

        public void Send<T>(NetPeer peer,EDeliveryMethod eMethod, T message) where T: Message
        {
            var (header, body) = Encode(message);
            var Data = new byte[header.Length + body.Length];
            Buffer.BlockCopy(src: header, 0, dst: Data, 0, header.Length);
            Buffer.BlockCopy(src: body, 0, dst: Data, header.Length, body.Length);
            peer.Send(
               Data,
               eMethod == EDeliveryMethod.Reliable ?
                   DeliveryMethod.ReliableOrdered :
                   DeliveryMethod.Unreliable);
        }

        //Returns with the bytes representing the length of the message and the message itself in bytes in two different arrays.
        protected (byte[] header, byte[] body) Encode<T>(T Message)
        {
            byte[] bodybytes = EncodeBody(Message);
            byte[] headerbytes = new byte[HEADER_SIZE]; 
            Array.Copy(BitConverter.GetBytes(bodybytes.Length),headerbytes,HEADER_SIZE);
            return (headerbytes, bodybytes);
        }

        //Reads the length of the message and returns with it.
        private int ReadHeader(byte [] buffer)
        {
            byte[] headerbytes = new byte[HEADER_SIZE];
            Array.Copy(buffer,headerbytes,HEADER_SIZE);
            return BitConverter.ToInt32(headerbytes, 0);
        }

        //Reads the body from the buffer and returns with the decoded message.
        private TMessageType ReadBody(byte[] buffer, int BodyLength)
        {
            byte[] BodyBytes= new byte[BodyLength];
            Array.Copy(buffer,HEADER_SIZE,BodyBytes,0,BodyLength);
            return Decode(BodyBytes);
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
