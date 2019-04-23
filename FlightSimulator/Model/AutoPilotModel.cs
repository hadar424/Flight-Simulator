using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    class AutoPilotModel
    {
        public void Send(string s)
        {
            string commandLine = "";
            while (s != "")
            {
                if (s.IndexOf("\n") == -1)
                {
                    CommandChannel.Instance.Send(s + "\r\n");
                    s = "";
                }
                else
                {
                    commandLine = s.Substring(0, s.IndexOf("\n") - 1);
                    s = s.Remove(0, s.IndexOf("\n") + 1);
                    CommandChannel.Instance.Send(commandLine + "\r\n");
                }
            }

        }
    }
}
