using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ruag.Client.Helpers;
using Ruag.Client.Helpers.Enums;
using Ruag.Client.Helpers.RuagEventArgs;
using System;
using Ruag.Common;
using Ruag.Common.Enums;
using Microsoft.Windows.Design.Interaction;
using System.Threading;

namespace Ruag.Client.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        private eUIMode _currentUIMode;
        private eLocales _currentLocale;

        private UIScreens _selectedScreen;
        public ViewModelBase MainContent { get; set; }
        public UIScreens SelectedScreen
        {
            get { return _selectedScreen; } 
            set
            {
                if (value == _selectedScreen)
                {
                    return;
                }
                _selectedScreen = value;
                ChangeScreen(value);
            }
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            InitializeCommands();
            RegisterGalasoftMessageHandlers();
            MainContent = new HomeViewModel();
            SelectedScreen = UIScreens.Home;
            RaisePropertyChanged("SelectedScreen");
        }

        private void RegisterGalasoftMessageHandlers()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<LocaleMessage>(this, LocaleChangeMsgHandler);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<UIModeMessage>(this, UIModeChangeMsgHandler);
        }

        #region Events
        
        public event EventHandler MinimizeEvent;
        public event EventHandler MaximizeEvent;
        public event EventHandler CloseEvent;
        public event EventHandler<LocaleChangeEventArgs> LocaleChagedEvent;
        public event EventHandler<ThemeChangeEventArgs> ThemeChangedEvent;
        
        #endregion

        #region Commands
        public RelayCommand MinimizeCommand { get; set; }
        public RelayCommand MaximizeCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
       
        #endregion


        public void UILoadEventHandler()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                MainContent = new HomeViewModel();
                RaisePropertyChanged("ContentViewModel");
                ThreadStart ts = new ThreadStart(HttpManager.Instance.Login);
                Thread thread = new Thread(ts);
                thread.Start();               
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
           
        }

        private void InitializeCommands()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                MinimizeCommand = new RelayCommand(MinimizeCommandHandler);
                MaximizeCommand = new RelayCommand(MaximizeCommandHandler);
                CloseCommand = new RelayCommand(CloseCommandHandler);
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
        }

        private void ChangeScreen(UIScreens screen)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            switch (screen)
            {
                default:
                case UIScreens.Home:
                    MainContent = new HomeViewModel();
                    break;
                case UIScreens.Roles:
                    MainContent = new RoleViewModel();
                    break;
                case UIScreens.Employees:
                    MainContent = new PersonViewModel();
                    break;
                case UIScreens.Settings:
                    SettingsViewModel Vm_Settings = new SettingsViewModel() {SelectedUIMode = _currentUIMode, SelectedLocale = _currentLocale };
                    MainContent = Vm_Settings;
                    break;
            }
            RaisePropertyChanged("MainContent");
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void CloseCommandHandler()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            if (CloseEvent != null)
            {
                CloseEvent(this, null);
            }
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void MaximizeCommandHandler()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            if (MaximizeEvent != null)
            {
                MaximizeEvent(this, null);
            }
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void MinimizeCommandHandler()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            if (MinimizeEvent != null)
            {
                MinimizeEvent(this, null);
            }
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void UIModeChangeMsgHandler(UIModeMessage obj)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            _currentUIMode = obj.UIMode;
            ThemeChangedEvent(this, new ThemeChangeEventArgs() { NewUIMode = obj.UIMode });
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }


        private void LocaleChangeMsgHandler(LocaleMessage obj)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            _currentLocale = obj.SelectedLocale;
            LocaleChagedEvent(this, new LocaleChangeEventArgs() { NewLocale = obj.SelectedLocale });
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }
}