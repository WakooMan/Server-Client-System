using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingSystem
{
    internal class Connection
    {
        private Socket OtherSocket;
        private bool IsServerSide;

        public Connection(bool IsServer,Socket socket)
        {
            this.IsServerSide = IsServer;
            this.OtherSocket = socket;
        }

        public bool ConnectToServer(string IPAddress,int Port)
        {
            //Server can't connect to server
            if (IsServerSide)
                return false;
            try
            {
                OtherSocket.Connect(IPAddress, Port);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void Disconnect()
        {
            OtherSocket.Disconnect(true);
        }

        public bool IsConnected()
        {
            return  OtherSocket.Connected;
        }

        public void Send<DataType>(DataType Data) where DataType : NetworkSerializable, new()
        {
            byte[] body = Data.Serialize();
            byte[] FullMsgLengthArray = BitConverter.GetBytes(body.Length);
            List<byte> FullMsg = FullMsgLengthArray.ToList();
            FullMsg.AddRange(body);
            OtherSocket.Send(FullMsg.ToArray());
        }

        public DataType Receive<DataType>(Socket Other) where DataType : NetworkSerializable,new()
        {
            byte[] Buffer = new byte[1024];
            OtherSocket.Receive(Buffer);
            byte[] Len = new byte[4];
            Array.Copy(Buffer, 0, Len, 0, 4);
            int FullLen = BitConverter.ToInt32(Len,0);
            byte[] Data = new byte[FullLen];
            Array.Copy(Buffer, 4, Data, 0, FullLen);
            DataType data = new DataType();
            data.Deserialize(Data);
            return data;
        }
    }
}
