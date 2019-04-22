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
            Thread thread = new Thread(() => {
                while (true)
                {
                    try
                    {
                        client = listener.AcceptSocket();
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
            thread.Start();
        }
        public void Stop()
        {
            listener.Stop();
        }

        public void ReadInfo(Socket client)
        {
            new Task(() =>
            {
                stream = new NetworkStream(client);
                reader = new StreamReader(stream);
                {
                    while(client.Connected)
                    {
                        string infoLine = reader.ReadLine();
                        //Console.WriteLine(infoLine);
                        HandleInfo(infoLine);
                    }
                }
            }).Start();
        }

        public void HandleInfo(string info)
        {   if(info !=null)
            {
                int first = info.IndexOf(",");
                int second = info.IndexOf(",", info.IndexOf(",") + 1);
                string lon = info.Substring(0, first);
                string lat = info.Substring(first + 1, second - first);
                /*
                FlightBoardViewModel flightBoard = new FlightBoardViewModel();
                flightBoard.Lon = Double.Parse(lon);
                flightBoard.Lat = Double.Parse(lat);*/
            }

        }


    }
}
