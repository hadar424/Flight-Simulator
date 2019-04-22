using FlightSimulator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightSimulator.ViewModels
{
    class AutoPilotViewModel : BaseNotify
    {
        AutoPilotModel model = new AutoPilotModel();
        public ICommand okButton;
        private ICommand clearButton;
        public string text = "";

        public string ChangeColor 
        {
            get
            {
                if(AutoPilotText == "")
                {
                    Console.WriteLine(AutoPilotText + "empty");
                    return "White";

                   
                } else
                {
                    return "LightSalmon";
                }
            }
        }



        public string AutoPilotText
        {
            set
            {
                text = value;
                NotifyPropertyChanged("AutoPilotText");
                NotifyPropertyChanged("ChangeColor");
            }
            get
            {
                return text;
            }
        }

        public ICommand OkButton
        {
            get
            {
                return okButton ?? (okButton = new CommandHandler(() => OnClick()));
            }
        }

        private void OnClick()
        {
            model.Send(AutoPilotText);
        }

        public ICommand ClearButton
        {
            get
            {
                return clearButton ?? (clearButton = new CommandHandler(() => OnClick1()));
            }
        }

        private void OnClick1()
        {
            AutoPilotText = "";
            NotifyPropertyChanged("AutoPilotText");
            
        }
    }
}
