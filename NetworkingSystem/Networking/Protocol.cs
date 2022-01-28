using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Networking{
    public abstract class Protocol<TMessageType>
    {
        //This is the size of an int in bytes.
        const int HEADER_SIZE = 5;

        public TMessageType Receive(byte[] bytes)
        {
            (int BodyLen,bool IsUnreliable) =ReadHeader(bytes);
            return ReadBody(bytes, BodyLen,IsUnreliable);
        }

        public void Send<T>(NetPeer peer,EDeliveryMethod eMethod, T message) where T: Message
        {
            var (header, body) = Encode<T>(message);
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
        protected (byte[] header, byte[] body) Encode<T>(T Message) where T : Message
        {
            byte[] bodybytes = EncodeBody(Message);
            byte[] headerbytes = new byte[5]; 
            Array.Copy(BitConverter.GetBytes(bodybytes.Length),headerbytes,4);
            bool IsUnreliable = typeof(T).IsSubclassOf(typeof(UnreliableMessage)) ? true : false;
            Array.Copy(BitConverter.GetBytes(IsUnreliable),0,headerbytes,4,1);
            return (headerbytes, bodybytes);
        }

        //Reads the length of the message and returns with it.
        private (int,bool) ReadHeader(byte [] buffer)
        {
            byte[] headerbytes = new byte[HEADER_SIZE];
            Array.Copy(buffer,headerbytes,HEADER_SIZE);
            return (BitConverter.ToInt32(headerbytes, 0),BitConverter.ToBoolean(buffer,4));
        }

        //Reads the body from the buffer and returns with the decoded message.
        private TMessageType ReadBody(byte[] buffer, int BodyLength,bool IsUnreliable)
        {
            byte[] BodyBytes= new byte[BodyLength];
            Array.Copy(buffer,HEADER_SIZE,BodyBytes,0,BodyLength);
            return Decode(BodyBytes,IsUnreliable);
        }

        //We assert if the header is valid.
        protected virtual void AssertValidMessageLength(int messageLength)
        {
            if (messageLength < 1)
                throw new ArgumentOutOfRangeException("Invalid Message Length");
        }

        //Decodes the message.
        protected abstract TMessageType Decode(byte[] Message,bool IsUnreliable);

        //Encodes the message.
        protected abstract byte[] EncodeBody<T>(T message) where T: Message;
    }
}
