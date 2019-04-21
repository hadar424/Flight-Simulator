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

        public void Throttle
        {
            set {
                manual.Send("/controls/engines/engine/throttle " + ToString);
            }
        }

    }
}
