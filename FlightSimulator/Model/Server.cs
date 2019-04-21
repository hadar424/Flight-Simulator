using FlightSimulator.Model.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    class Server
    {
        private int port;
        private TcpListener listener;
        private IClientHandler ch;
        private StreamReader reader;
        private NetworkStream stream;
        private string ip;
        public Server(int port, IClientHandler ch, string IP)
        {
            this.port = port;
            this.ch = ch;
            ip = IP;
            Start();
        }
        public void Start()
        {

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            listener = new TcpListener(ep);
            listener.Start();
            Console.WriteLine("Waiting for connections...");
            Thread thread = new Thread(() => {
                while (true)
                {
                    try
                    {
                        Console.WriteLine("while loop");
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");
                        ch.HandleClient(client);

                
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            thread.Start();
        }
        public void Stop()
        {
            listener.Stop();
        }




    }
}
