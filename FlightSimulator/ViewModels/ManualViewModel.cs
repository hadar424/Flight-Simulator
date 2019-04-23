using FlightSimulator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Views;
namespace FlightSimulator.ViewModels
{
    class ManualViewModel
    {
        ManualModel manual = new ManualModel();

        public double Throttle
        {
            set
            {
                manual.Send("set controls/engines/current-engine/throttle " + Math.Round(value, 2).ToString());
            }
        }

        public double Rudder
        {
            set
            {
                manual.Send("set controls/flight/rudder " + Math.Round(value, 2).ToString());
            }
        }

    }
}
