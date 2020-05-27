using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ruag.Client.Helpers;
using Ruag.Client.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ruag.Common;
using Ruag.Common.Enums;

namespace Ruag.Client.ViewModel
{
    public class SettingsViewModel:ViewModelBase
    {

        private eUIMode _selectedUIMode;
        public eUIMode SelectedUIMode
        {
            get
            {
                return _selectedUIMode;
            }

            set
            {
                if (value != _selectedUIMode)
                {
                    ModeChanged(value);
                }
                _selectedUIMode = value;
                
            }

        }

        private eLocales _selectedLocale { get; set; }
        public eLocales SelectedLocale
        {
            get
            {
                return _selectedLocale;
            }

            set
            {
                if (value != _selectedLocale)
                {
                    LocaleChanged(value);
                }
                _selectedLocale = value;
              
            }

        }

        public SettingsViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void LocaleChanged(eLocales locale)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<LocaleMessage>(new LocaleMessage() { SelectedLocale = locale }); ;
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void ModeChanged(eUIMode uIMode)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<UIModeMessage>(new UIModeMessage() { UIMode = uIMode });
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void ColorChangedCommandHandler()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<UIColorMessage>(new UIColorMessage());
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }
}
