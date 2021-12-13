using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkingSystem
{
    /*
    public class Server
    {
        private ManualResetEvent allDone = new ManualResetEvent(false);
        private Socket server;
        private List<Connection> connections;
        private bool run;

        private Server()
        {
            // Get Host IP Address that is used to establish a connection
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1
            // If a host has multiple addresses, you will get a list of addresses
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            run = true;
            // Create a Socket that will use Tcp protocol
            server = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // A Socket must be associated with an endpoint using the Bind method

            StartListening(localEndPoint);
            connections.Add(new Connection(true,hostClient));


        }

        private void StartListening(IPEndPoint localEndPoint)
        {
            try
            {
                // A Socket must be associated with an endpoint using the Bind method
                server.Bind(localEndPoint);
                // Specify how many requests a Socket can listen before it gives Server busy response.
                // We will listen 10 requests at a time
                server.Listen(100);

                while (run)
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    Console.WriteLine("Waiting for a connection...");
                    server.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        server);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Add Handler to the Connection list  
            connections.Add(new Connection(true,handler));
            OnClientConnect();
        }


        public void Stop()
        {
            foreach (Connection conn in connections)
            {
                conn.Disconnect();
            }
            server.Close();
            server.Shutdown(SocketShutdown.Both);
        }

        public void OnClientConnect()
        { 

        }

        public void OnClientDisconnect()
        {
            
        }
    }*/
}
