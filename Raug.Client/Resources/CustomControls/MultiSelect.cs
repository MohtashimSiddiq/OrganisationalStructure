using Microsoft.Windows.Design.Interaction;
using Ruag.Client.Helpers;
using Ruag.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ruag.Client.Resources.CustomControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Ruag.Client.Resources.CustomControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Ruag.Client.Resources.CustomControls;assembly=Ruag.Client.Resources.CustomControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:MultiSelect/>
    ///
    /// </summary>
    public class RoleSelector : Control
    {
        #region Fields
        public bool ExecuteSelctionChangedCallBack { get; set; }
        public bool IsHandlingSelectionChangedCmd = false;
        #endregion

        #region routed commands
        private static RoutedCommand _addCommand;

        public static RoutedCommand AddCommand
        {
            get { return _addCommand; }
        }

        private static RoutedCommand _removeCommand;

        public static RoutedCommand RemoveCommand
        {
            get { return _removeCommand; }
        }

        private static RoutedCommand _selectionChangedCommand;

        public static RoutedCommand SelectionChangedCommand
        {
            get { return _selectionChangedCommand; }
        }
        #endregion routed commands






        static RoleSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoleSelector), new FrameworkPropertyMetadata(typeof(RoleSelector)));
            _addCommand = new RoutedCommand("AddCommand", typeof(RoleSelector));
            _selectionChangedCommand = new RoutedCommand("SelectionChangedCommand", typeof(RoleSelector));
            _removeCommand = new RoutedCommand("RemoveCommand", typeof(RoleSelector));
            CommandManager.RegisterClassCommandBinding(typeof(RoleSelector), new CommandBinding(_addCommand, AddCommandHandler));
            CommandManager.RegisterClassCommandBinding(typeof(RoleSelector), new CommandBinding(_selectionChangedCommand, SelectionChangedCommandHandler));
            CommandManager.RegisterClassCommandBinding(typeof(RoleSelector), new CommandBinding(_removeCommand, RemoveCommandHandler));

        }

        public RoleSelector()
        {
            ExecuteSelctionChangedCallBack = false;
        }

    private static void SelectionChangedCommandHandler(object sender, ExecutedRoutedEventArgs e)
    {
            RoleSelector ms = sender as RoleSelector;
            if (ms.IsHandlingSelectionChangedCmd)
            {
                return;
            }
            ms.ExecuteSelctionChangedCallBack = false;
            ms.IsHandlingSelectionChangedCmd = true;
            ObservableCollection<MultiSelectItem<OrgRoleDTO>> tempSelecItems = ms.GetValue(SelectionListProperty) as ObservableCollection<MultiSelectItem<OrgRoleDTO>>;
            OrgRoleDTO cmdParam = e.Parameter as OrgRoleDTO;
            var removedItem = tempSelecItems.ToList().Where(item => item.Items.Contains(cmdParam)).First();
            int removedItemIndex = tempSelecItems.IndexOf(removedItem);
            for (int i = removedItemIndex + 1; i < tempSelecItems.Count;)
            {

                tempSelecItems.RemoveAt(i);
            }

            if (cmdParam.ChildRoles.Count == 0)
            {
                if (tempSelecItems.Last().IsButton)
                    tempSelecItems.RemoveAt(tempSelecItems.Count - 1);
            }
            else
            {
                if(!tempSelecItems.Last().IsButton)
                tempSelecItems.Add(new MultiSelectItem<OrgRoleDTO>() { IsButton = true });
            }
            ms.SetValue(SelectionProperty, cmdParam);
            ms.SetValue(SelectionListProperty, tempSelecItems);

            ms.ExecuteSelctionChangedCallBack = true;
            ms.IsHandlingSelectionChangedCmd = false;
            //Selection
        }

        private static void RemoveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            RoleSelector ms = sender as RoleSelector;
            OrgRoleDTO param = e.Parameter as OrgRoleDTO;
            if (param.ParentRoleId == 0)
            {
                return;
            }
            ObservableCollection<MultiSelectItem<OrgRoleDTO>> tempSelecItems = ms.GetValue(SelectionListProperty) as ObservableCollection<MultiSelectItem<OrgRoleDTO>>;
            var removedItem = tempSelecItems.ToList().Where(item => item.CurrentItem.Id == param.Id).First();
            int removedItemIndex = tempSelecItems.IndexOf(removedItem);
            for (int i = removedItemIndex; i < tempSelecItems.Count;)
            {

                tempSelecItems.RemoveAt(i);
            }
            if (tempSelecItems.Last().CurrentItem.ChildRoles.Count > 0)
            {
                 tempSelecItems.Add(new MultiSelectItem<OrgRoleDTO>() { IsButton = true });
            }
            ms.SetValue(SelectionListProperty, tempSelecItems);


        }

        


        private static void AddCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            
            RoleSelector ms = sender as RoleSelector;
            ms.ExecuteSelctionChangedCallBack = false;
            ObservableCollection<MultiSelectItem<OrgRoleDTO>> tempSelecItems = ms.GetValue(SelectionListProperty) as ObservableCollection<MultiSelectItem<OrgRoleDTO>>;
            OrgRoleDTO LastItem = tempSelecItems[tempSelecItems.Count - 2 ].CurrentItem;
            if (LastItem != null)
            {
                MultiSelectItem<OrgRoleDTO> tempSelction = new MultiSelectItem<OrgRoleDTO>()
                {
                    Items = new ObservableCollection<OrgRoleDTO>(LastItem.ChildRoles),
                    CurrentItem = LastItem.ChildRoles.First()
                };
                tempSelecItems.Insert(tempSelecItems.Count-1, tempSelction);
                ms.SetValue(SelectionProperty, LastItem.ChildRoles.First());
                if (tempSelction.CurrentItem.ChildRoles.Count == 0)
                {
                    tempSelecItems.RemoveAt(tempSelecItems.Count - 1);
                }
                ms.SetValue(SelectionListProperty, tempSelecItems);
            }
            ms.ExecuteSelctionChangedCallBack = true;
        }

        #region Dependency Props
        

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(IEnumerable<OrgRoleDTO>), typeof(RoleSelector), new PropertyMetadata(new PropertyChangedCallback(SourceChanged)));

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RoleSelector ms = d as RoleSelector;
            if (e.NewValue == null)
            {
                return;
            }
            
            AddInitial(e.NewValue as ObservableCollection<OrgRoleDTO>, ms);
            ObservableCollection<MultiSelectItem<OrgRoleDTO>> tempSelecItems = ms.GetValue(SelectionListProperty) as ObservableCollection<MultiSelectItem<OrgRoleDTO>>;
            ms.ExecuteSelctionChangedCallBack = true;
            ms.SetValue(SelectionProperty, tempSelecItems[0].CurrentItem);
          
        }

        private static void AddInitial(ObservableCollection<OrgRoleDTO> source, RoleSelector ms)
        {
            if (source != null && source.Count > 0)
            {
                MultiSelectItem<OrgRoleDTO> tempSelction = new MultiSelectItem<OrgRoleDTO>()
                {
                    Items = new ObservableCollection<OrgRoleDTO>() { source.First() },
                    CurrentItem = source.First()
                };

                if (source.First().ChildRoles.Count > 0)
                {
                    ms.SetValue(SelectionListProperty, new ObservableCollection<MultiSelectItem<OrgRoleDTO>>() { tempSelction, new MultiSelectItem<OrgRoleDTO>() { IsButton = true } }); ;
                }
                
            }
        }


        public IEnumerable<OrgRoleDTO> Source
        {
            get { return (IEnumerable<OrgRoleDTO>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SelectionProperty = DependencyProperty.Register("Selection", typeof(OrgRoleDTO),
            typeof(RoleSelector), new PropertyMetadata( new OrgRoleDTO() ,new PropertyChangedCallback(SelectionChangedCallBack)));


        private static void SelectionChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RoleSelector ms = d as RoleSelector;
            if (!ms.ExecuteSelctionChangedCallBack)
            {
                var x = ms.GetValue(SelectionProperty);
                ms.ExecuteSelctionChangedCallBack = true;
                return;
            }

            OrgRoleDTO newRole = e.NewValue as OrgRoleDTO;
            ObservableCollection<OrgRoleDTO> sourceItems = ms.GetValue(SourceProperty) as ObservableCollection<OrgRoleDTO>;
            if (sourceItems == null )
            {
                return;
            }
            ObservableCollection<MultiSelectItem<OrgRoleDTO>> tempSelctionList = ms.GetValue(SelectionListProperty) as ObservableCollection<MultiSelectItem<OrgRoleDTO>>;
            if (tempSelctionList != null)
            {
                tempSelctionList.Clear();
            }

                
            
            if (newRole == null || newRole.Id == 0)
            {
                AddInitial(sourceItems, ms);
                return;
            }

            OrgRoleDTO currentParentValue = e.NewValue as OrgRoleDTO;
            //Clearing current selection list
            ObservableCollection<MultiSelectItem<OrgRoleDTO>> tempSelectnList = ms.GetValue(SelectionListProperty) as ObservableCollection<MultiSelectItem<OrgRoleDTO>>;
            if (tempSelectnList != null)
            {
                tempSelectnList.Clear();
            }
            //Fetching Orginal list
            
            currentParentValue = (from role in sourceItems where role.Id == currentParentValue.Id select role).FirstOrDefault();
            ObservableCollection<MultiSelectItem<OrgRoleDTO>> tempSelecItems = new  ObservableCollection<MultiSelectItem<OrgRoleDTO>>();
            while(currentParentValue!=null)
            {
                List<OrgRoleDTO> currentParentSiblings = (from role in sourceItems where role.ParentRoleId == currentParentValue.ParentRoleId select role).ToList();
                tempSelecItems.Add(new MultiSelectItem<OrgRoleDTO>() { CurrentItem = currentParentValue, IsButton = false, Items = new ObservableCollection<OrgRoleDTO>(currentParentSiblings)  });
                currentParentValue = (from role in sourceItems where role.Id == currentParentValue.ParentRoleId select role).FirstOrDefault();
            }
            tempSelecItems = new ObservableCollection<MultiSelectItem<OrgRoleDTO>> (tempSelecItems.Reverse());
            if (tempSelecItems.Last().CurrentItem.ChildRoles.Count > 0)
            {
                tempSelecItems.Add(new MultiSelectItem<OrgRoleDTO>() { IsButton = true });
            }
            
            ms.SetValue(SelectionListProperty, tempSelecItems);

        }




        public OrgRoleDTO Selection
        {
            get
            {
                return (OrgRoleDTO)GetValue(SelectionProperty); 
            }
            set
            {
                SetValue(SelectionProperty, value);
            }
        }


        public static readonly DependencyProperty SelectionListProperty = DependencyProperty.Register("SelectionList", typeof(ObservableCollection<MultiSelectItem<OrgRoleDTO>>), typeof(RoleSelector));

        public IEnumerable< MultiSelectItem<OrgRoleDTO>> SelectionList
        {
            get { return (ObservableCollection<MultiSelectItem<OrgRoleDTO>>)GetValue(SelectionProperty); }
            set { SetValue(SelectionListProperty, value); }
        }

        #endregion Dependency Props

    }
}
