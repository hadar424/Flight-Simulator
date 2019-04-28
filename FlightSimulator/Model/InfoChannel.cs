using FlightSimulator.ViewModels;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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
            // create new thread for the information channel
            infoThread = new Thread(() =>
            {
                while (!closeThread)
                {
                    try
                    {
                        client = listener.AcceptSocket();
                        // read information from the simulator
                        ReadInfo(client);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                listener.Stop();
            });
            infoThread.Start();
        }

        public void ReadInfo(Socket client)
        {
            string info = "";
            if (client.Connected)
            {
                stream = new NetworkStream(client);
                reader = new StreamReader(stream);
                while (!closeThread)
                {
                    // read one line
                    info = reader.ReadLine();
                    HandleInfo(info);
                    // initialize info buffer
                    info = "";

                }
            }
        }
        public void HandleInfo(string info)
        {
            if (info != null)
            {
                int first = info.IndexOf(",");
                int second = info.IndexOf(",", info.IndexOf(",") + 1);
                // get the substring of the lon value
                string lon = info.Substring(0, first - 1);
                // get the substring of the lat value
                string lat = info.Substring(first + 1, second - first - 1);
                // convert lon from string to float
                FlightBoardViewModel.Instance.Lon = float.Parse(lon);
                // convert lat from string to float
                FlightBoardViewModel.Instance.Lat = float.Parse(lat);
            }
        }

        public void Disconnect()
        {
            if(client.Connected)
            {
                closeThread = true;
                // close the thread
                if (infoThread != null)
                {
                    infoThread.Abort();
                }
                CommandChannel.Instance.Disconnect();
                // close the socket
                client.Close();
            }
        }
    }
}
