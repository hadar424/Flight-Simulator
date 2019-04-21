using FlightSimulator.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.ViewModels
{
    public class FlightBoardViewModel : BaseNotify
    {

        public double Lon
        {
            get
            {
                return Lon;
            }
            set
            {
                Lon = value;
                NotifyPropertyChanged("Lon");
            }
        }

        public double Lat
        {
            get
            {
                return Lat;
            }
            set
            {
                Lat = value;
                NotifyPropertyChanged("Lat");
            }
        }
    }
}
