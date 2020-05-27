using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruag.Client.Helpers
{
    public class MultiSelectItem<T>:INotifyPropertyChanged
    {

        private T _currentItem;
        public T CurrentItem
        {
            get
            {
                return _currentItem;
            }
            set
            {
                _currentItem = value;
                RaisePropertyChanged("CurrentItem");
            }
        }

        private ObservableCollection<T> _items;
        public ObservableCollection<T> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                RaisePropertyChanged("Items");
            }
        }

        private bool _isButton = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsButton
        {
            get
            {
                return _isButton;
            }
            set
            {
                _isButton = value;
                RaisePropertyChanged("IsButton");
            }
        }

        private void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
