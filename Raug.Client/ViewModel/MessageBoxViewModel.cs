using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ruag.Client.Helpers;
using Ruag.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ruag.Client.ViewModel
{
    public class MessageBoxViewModel:ViewModelBase
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public eMessageBoxType Type { get; set; }

        public RelayCommand OkCommand { get; set; }
        public RelayCommand YesCommand { get; set; }
        public RelayCommand NoCommand { get; set; }

        public MessageBoxViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            OkCommand = new RelayCommand(OkCommandHandler);
            YesCommand = new RelayCommand(YesCommandHandler);
            NoCommand = new RelayCommand(NoCommandHandler);



        }

        private void NoCommandHandler()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<MsgBxResultMessage>(new MsgBxResultMessage() { Result = eMessageBoxResult.No });
        }

        private void OkCommandHandler()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<MsgBxResultMessage>(new MsgBxResultMessage() { Result = eMessageBoxResult.Ok });
        }

        private void YesCommandHandler()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<MsgBxResultMessage>(new MsgBxResultMessage() { Result = eMessageBoxResult.Yes });
        }
    }
}
