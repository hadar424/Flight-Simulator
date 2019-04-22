using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    class ManualModel
    {

        public void Send(string s)
        {
            CommandChannel.Instance.Send(s);
        }

       
    }
}
