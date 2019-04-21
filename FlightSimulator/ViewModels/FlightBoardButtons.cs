using FlightSimulator.Model;
using FlightSimulator.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightSimulator.ViewModels
{
    class FlightBoardButtons
    {
        private ICommand settingsCommand;
        private ICommand connectCommand;
        private bool isConnected = false;
        public ICommand SettingsCommand
        {
            get
            {
                return settingsCommand ?? (settingsCommand = new CommandHandler(() => OnClick()));
            }
        }

        private void OnClick()
        {
            SettingsWin settings = new SettingsWin();
            settings.ShowDialog();

        }

        public ICommand ConnectCommand
        {
            get
            {
                return connectCommand ?? (connectCommand = new CommandHandler(() => OnConnect()));
            }
        }

        private void OnConnect()
        {
            int infoPort = ApplicationSettingsModel.Instance.FlightInfoPort;
            int commandoPort = ApplicationSettingsModel.Instance.FlightCommandPort;
            string serverIP = ApplicationSettingsModel.Instance.FlightServerIP;
            ClientHandler myClient = new ClientHandler();
            Server myServer = new Server(infoPort, myClient);
            
        }
    }
}
