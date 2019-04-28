using FlightSimulator.Model;
using System;
namespace FlightSimulator.ViewModels
{
    class ManualViewModel
    {
        // Throttle property
        public double Throttle
        {
            set
            {
                // send throttle value to the simulator 
                CommandChannel.Instance.Send("set controls/engines/current-engine/throttle " + Math.Round(value, 2).ToString());
            }
        }

        // Rudder property
        public double Rudder
        {
            set
            {
                // send rudder value to the simulator 
                CommandChannel.Instance.Send("set controls/flight/rudder " + Math.Round(value, 2).ToString());
            }
        }

    }
}
