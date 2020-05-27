using GalaSoft.MvvmLight;
using Ruag.Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ruag.Client.Helpers.DataTemplateSelectors
{
    public class MainContentSelector : DataTemplateSelector
    {
        public DataTemplate HomeTemplate { get; set; }
        public DataTemplate SettingsTemplate { get; set; }
        public DataTemplate RolesTemplate { get; set; }
        public DataTemplate PersonTemplate { get; set; }

        public MainContentSelector()
        {
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return HomeTemplate;
            }

            ViewModelBase vm = (ViewModelBase)item;
            if (vm is RoleViewModel)
            {
                return RolesTemplate;
            }
            else if (vm is SettingsViewModel)
            {
                return SettingsTemplate;
            }
            else if (vm is PersonViewModel)
            {
                return PersonTemplate;
            }
            else
            {
                return HomeTemplate;
            }
        }
    }
}
