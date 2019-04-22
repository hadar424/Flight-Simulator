using FlightSimulator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FlightSimulator.ViewModels
{
    class ManualViewModel
    {
        ManualModel manual = new ManualModel();

        public double Throttle
        {
            set
            {
                manual.Send("set /controls/engines/engine/throttle " + Math.Round(value, 2).ToString() + "\r\n");
            }
        }

        public double Rudder
        {
            set
            {
                manual.Send("set /controls/flight/rudder " + Math.Round(value, 2).ToString() + "\r\n");
            }
        }

        public double Aileron
        {
            set
            {
                manual.Send("set /controls/flight/aileron " + Math.Round(value, 2).ToString() + "\r\n");
            }
        }

        public double Elevator
        {
            set
            {
                manual.Send("set /controls/flight/elevator " + Math.Round(value, 2).ToString() + "\r\n");
            }
        }

    }
}
