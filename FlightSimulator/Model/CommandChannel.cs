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
            commandThread = new Thread(() =>
            {
            Console.WriteLine("waiting for connect");
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            while (!client.Connected)
            {
                try
                {
                    client.Connect(ep);
                }
                catch (SocketException)
                {

                }

            }
            stream = new NetworkStream(client);
            Console.WriteLine("connected");
            Console.WriteLine(client.Connected);
         
            });
            commandThread.Start();

        }

        public void Send(string s)
        {
            sendThread = new Thread(() =>
            {
                if (client.Connected)
                {
                    string commandLine = "";
                    while (s != "")
                    {
                        if (s.IndexOf("\n") == -1)
                        {
                            Console.WriteLine("-last: " + s + "-\n");
                            Byte[] buffer = Encoding.ASCII.GetBytes(s + "\r\n");
                            stream.Write(buffer, 0, buffer.Length);
                            s = "";
                        }
                        else
                        {
                            commandLine = s.Substring(0, s.IndexOf("\n") - 1);
                            s = s.Remove(0, s.IndexOf("\n") + 1);
                            Console.Write("command: " + commandLine + "-\n");
                            Console.Write("remain: " + s + "-\n");
                            // לעשות בדיקות קלט לפני שליחה
                            Byte[] buffer = Encoding.ASCII.GetBytes(commandLine + "\r\n");
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
                if(sendThread != null)
                {
                    sendThread.Abort();
                }
                if (commandThread != null)
                {
                    commandThread.Abort();
                }
                client.Close();
            }

        }



    }
}
