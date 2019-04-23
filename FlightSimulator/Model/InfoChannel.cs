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
            Thread thread = new Thread(() =>
            {
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
            string info = "";
            new Task(() =>
            {
                stream = new NetworkStream(client);
                reader = new StreamReader(stream);
                {
                    while (client.Connected)
                    {
                        info = reader.ReadLine();
                        HandleInfo(info);
                    }
                }
            }).Start();
        }

        public void HandleInfo(string info)
        {
            if (info != null)
            {
                int first = info.IndexOf(",");
                int second = info.IndexOf(",", info.IndexOf(",") + 1);
                string lon = info.Substring(0, first - 1);
                string lat = info.Substring(first + 1, second - first - 1);

                FlightBoardViewModel.Instance.Lon = Convert.ToDouble(lon);
                FlightBoardViewModel.Instance.Lat = Convert.ToDouble(lat);
                FlightBoardViewModel.Instance.NotifyPropertyChanged("Lon");
                FlightBoardViewModel.Instance.NotifyPropertyChanged("Lat");

            }


        }
    }
}
