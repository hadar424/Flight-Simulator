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
    class CommandChannel
    {
        private int port;
        private string ip;
        private NetworkStream stream;
        private Socket client;
        private Thread commandThread;
        private Thread sendThread;

        #region Singleton
        private static CommandChannel m_Instance = null;
        public static CommandChannel Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new CommandChannel();
                }
                return m_Instance;
            }
        }
        #endregion

        public int CommandPort
        {
            get { return port; }
            set { port = value; }
        }

        public string ServerIP
        {
            get { return ip; }
            set { ip = value; }
        }

        public Thread CommandThread
        {
            get { return commandThread; }
            set { commandThread = value; }
        }


        public void Connect()
        {
            // create new thread for the commands chanel
            commandThread = new Thread(() =>
            {
                // create new socket by the given ip and port
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                while (!client.Connected)
                {
                    try
                    {
                        // try to connect to the simulator as client
                        client.Connect(ep);
                    }
                    catch (SocketException)
                    {
                    }

            }
            stream = new NetworkStream(client);
            });
            commandThread.Start();

        }

        public void Send(string s)
        {
            // create new thread for sending massages
            sendThread = new Thread(() =>
            {
                if (client.Connected)
                {
                    string commandLine = "";
                    while (s != "")
                    {
                        // if there is one massage left to send
                        if (s.IndexOf("\n") == -1)
                        {
                            Byte[] buffer = Encoding.ASCII.GetBytes(s + "\r\n");
                            // send the massage to the simulator
                            stream.Write(buffer, 0, buffer.Length);
                            s = "";
                        }
                        // there is several massages to send
                        else
                        {
                            commandLine = s.Substring(0, s.IndexOf("\n") - 1);
                            s = s.Remove(0, s.IndexOf("\n") + 1);
                            Byte[] buffer = Encoding.ASCII.GetBytes(commandLine + "\r\n");
                            // send the massage to the simulator
                            stream.Write(buffer, 0, buffer.Length);
                            Thread.Sleep(2000);
                        }
                    }
                }
            });
            sendThread.Start();

        }

        public void Disconnect()
        {
          if(client.Connected)
            {
                // close threads
                if(sendThread != null)
                {
                    sendThread.Abort();
                }
                if (commandThread != null)
                {
                    commandThread.Abort();
                }
                // close socket
                client.Close();
            }

        }



    }
}
