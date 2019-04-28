using FlightSimulator.Model;
using FlightSimulator.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightSimulator.ViewModels
{
    class FlightBoardButtons
    {
        private ICommand settingsCommand;
        private ICommand connectCommand;
        private ICommand disconnectCommand;
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
            // get info channel port
            int infoPort = ApplicationSettingsModel.Instance.FlightInfoPort;
            // get command channel port
            int commandoPort = ApplicationSettingsModel.Instance.FlightCommandPort;
            // get server ip
            string serverIP = ApplicationSettingsModel.Instance.FlightServerIP;
            // create and set data to info channel
            InfoChannel.Instance.InfoPort = infoPort;
            InfoChannel.Instance.ServerIP = serverIP;
            InfoChannel.Instance.Start();
            // create and set data to command channel
            CommandChannel.Instance.ServerIP = serverIP;
            CommandChannel.Instance.CommandPort = commandoPort;
            CommandChannel.Instance.Connect();
        }

        public ICommand DisconnectCommand
        {
            get
            {
                return disconnectCommand ?? (disconnectCommand = new CommandHandler(() => OnDisconnect()));
            }
        }

        private void OnDisconnect()
        {
            InfoChannel.Instance.Disconnect();

        }
    }
}
