using FlightSimulator.Model.Interface;
using FlightSimulator.ViewModels;
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
    class InfoChannel
    {
        private int port;
        private TcpListener listener;
        private StreamReader reader;
        private NetworkStream stream;
        private string ip;
        private Socket client;
        private volatile bool closeThread = false;
        private Thread infoThread;

        #region Singleton
        private static InfoChannel m_Instance = null;
        public static InfoChannel Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new InfoChannel();
                }
                return m_Instance;
            }
        }
        #endregion

        public string ServerIP
        {
            get { return ip; }
            set { ip = value; }
        }

        public Thread InfoThread
        {
            get { return infoThread; }
            set { infoThread = value; }
        }

        public int InfoPort
        {
            get { return port; }
            set { port = value; }
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            listener = new TcpListener(ep);
            listener.Start();
            Console.WriteLine("Waiting for connections...");
            infoThread = new Thread(() =>
            {
                while (!closeThread)
                {
                    try
                    {
                        client = listener.AcceptSocket();
                        Console.WriteLine(client);
                        Console.WriteLine("Got new connection");
                        ReadInfo(client);


                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            infoThread.Start();
        }
        public void Stop()
        {
            listener.Stop();
        }

        public void ReadInfo(Socket client)
        {
            string info = "";

                if (client.Connected)
                {
                    stream = new NetworkStream(client);

                    reader = new StreamReader(stream);
                    {
                        while (!closeThread)
                        {

                            info = reader.ReadLine();
                            Console.Write(info);
                            HandleInfo(info);
                            info = "";

                        }
                    }
                }

            

        }

        public void HandleInfo(string info)
        {
            if (info != null)
            {
                int first = info.IndexOf(",");
                int second = info.IndexOf(",", info.IndexOf(",") + 1);
                string lon = info.Substring(0, first - 1);
                string lat = info.Substring(first + 1, second - first - 1);

                FlightBoardViewModel.Instance.Lon = float.Parse(lon);
                FlightBoardViewModel.Instance.Lat = float.Parse(lat);

            }


        }

        public void Disconnect()
        {
            if(client.Connected)
            {
                closeThread = true;
                if (infoThread != null)
                {
                    infoThread.Abort();
                }
                CommandChannel.Instance.Disconnect();
                client.Close();
            
            }

            if(!client.Connected)
            {
                Console.WriteLine("disconnected");
            }

        }
    }
}
