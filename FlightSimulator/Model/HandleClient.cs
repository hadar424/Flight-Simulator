using FlightSimulator.Model.Interface;
using FlightSimulator.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace FlightSimulator.Model
{
    class ClientHandler : IClientHandler
    {
        private StreamWriter writer;
        private StreamReader reader;
        private NetworkStream stream;

        public ClientHandler()
        {

        }

        public void HandleClient(TcpClient client)
        {
    
            new Task(() =>
            {

                char[] buffer = new char[1200];
                using (stream = client.GetStream())
                using (reader = new StreamReader(stream))
                using (writer = new StreamWriter(stream))
                {
                    string commandLine = reader.ReadLine();
                    
                    int readIndex = 0;
                    readIndex = reader.Read(buffer, 0, 1200);
                    if (readIndex != -1)
                    {
                        string s = string.Join("", buffer);
                        ExecuteCommand(s, client);
                    }
                }
                client.Close();
            }).Start();
        }

        private void ExecuteCommand(string commandLine, TcpClient client)
        {
            int first = commandLine.IndexOf(",");

            int second = commandLine.IndexOf(",", commandLine.IndexOf(",") + 1);
            string lon = commandLine.Substring(0, first);
            string lat = commandLine.Substring(first, second - first);
            FlightBoardViewModel flightBoard = new FlightBoardViewModel();
            flightBoard.Lon = Double.Parse(lon);
            flightBoard.Lat = Double.Parse(lat);
        }

        public void Send(string s)
        {
            // לעשות בדיקות קלט לפני שליחה
            writer.Write(s);
        }
        public string pharseBuffer(char[] buffer)
        {
            string s = string.Join("", buffer);
            int first = s.IndexOf(",");

            int second = s.IndexOf(",", s.IndexOf(",") + 1);
            string lon = s.Substring(0, first);
            string lat = s.Substring(first, second - first);

            return s;
        }
    }
}
