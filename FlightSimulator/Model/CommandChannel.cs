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
        private StreamWriter writer;
        private NetworkStream stream;
        private Socket client;

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


        public void Connect()
        {
            Thread thread = new Thread(() =>
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
            writer = new StreamWriter(stream);
            Console.WriteLine("connected");
            Console.WriteLine(client.Connected);
         
            });
            thread.Start();

        }

        public void Send(string s)
        {
            new Task(() =>
            {
                if (client.Connected)
                {
                    Console.WriteLine(s);
                    // לעשות בדיקות קלט לפני שליחה
                    writer.Write(s);
                }
            }).Start();

        }

    }
}
