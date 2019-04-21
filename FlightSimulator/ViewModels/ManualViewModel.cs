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
            set {
                manual.Send("/controls/engines/engine/throttle " + value.ToString());
            }
        }

        public double Rudder
        {
            set
            {
                manual.Send("/controls/flight/rudder " + value.ToString());
            }
        }

        public double Aileron
        {
            set
            {
                manual.Send("/controls/flight/aileron " + value.ToString());
            }
        }

        public double Elevator
        {
            set
            {
                manual.Send("/controls/flight/elevator " + value.ToString());
            }
        }

    }
}
