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
        //AutoPilotModel model = new AutoPilotModel();
        public ICommand okButton;
        private ICommand clearButton;
        public string text = "";
        private bool IsWhite = false;

        public string ChangeColor 
        {
            get
            {
                if(AutoPilotText == "")
                {
                    return "White";
                    
                } else
                {
                    if(IsWhite)
                    {
                        IsWhite = false;
                        return "White";
                    }
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
            IsWhite = true;
            NotifyPropertyChanged("ChangeColor");
            CommandChannel.Instance.Send(AutoPilotText);
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
            IsWhite = true;
            NotifyPropertyChanged("AutoPilotText");
            
        }
    }
}
