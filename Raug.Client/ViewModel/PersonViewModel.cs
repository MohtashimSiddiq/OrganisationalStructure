using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Ruag.Client.Helpers;
using Ruag.Client.Helpers.Enums;
using Ruag.Common;
using Ruag.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using Ruag.Common.Enums;
using System.Collections.ObjectModel;
using System.Windows;
using System.Threading;
using GalaSoft.MvvmLight.Messaging;

namespace Ruag.Client.ViewModel
{
    public class PersonViewModel: ViewModelBase
    {
        public bool GridDisplaySelected { get; set; }

        public bool _isEdit = false;
        public string PageTitle { get; set; }
        public EmployeeDTO _selectedEmployee;
        public EmployeeDTO SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                RaisePropertyChanged("SelectedEmployee");
            }
        }

        public ObservableCollection<EmployeeDTO> AllEmployees { get; set; }
        public ObservableCollection<OrgRoleDTO> AllRoles { get; set; }

        public ObservableCollection<EmployeeDTO> TreeData { get; set; }

        #region Commands
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand<int> EditCommand { get; set; }
        public RelayCommand<int> DeleteCommand { get; set; }
        public EmployeeDTO RootEmployee { get; private set; }
        #endregion
        public PersonViewModel()
        {
            InitializeCommands();
           
            GridDisplaySelected = true;
            RaisePropertyChanged("GridDisplaySelected");
            PageTitle = Application.Current.MainWindow.Resources["txtAddEmployee"].ToString();
            RaisePropertyChanged("PageTitle");
            
            SelectedEmployee = new EmployeeDTO();
            RaisePropertyChanged("SelectedEmployee");
            ThreadStart ts = new ThreadStart(FetchData);
            Thread t = new Thread(ts);
            ts.BeginInvoke(null,null);
        }

        private void FetchData()
        {
            AllEmployees = new ObservableCollection<EmployeeDTO>(GetAllActiveEmployees());
            AllRoles = new ObservableCollection<OrgRoleDTO>(GetAllRoles());
            RaisePropertyChanged("AllEmployees");
            RaisePropertyChanged("AllRoles");
            ConstructTree();
        }

        private void ConstructTree()
        {
            OrgRoleDTO rootNode = (from role in AllRoles where role.ParentRoleId == 0 select role).FirstOrDefault();
            RootEmployee = CreateChild(rootNode);
            TreeData = new ObservableCollection<EmployeeDTO>();
            TreeData.Add(RootEmployee);
            RaisePropertyChanged("TreeData");
        }


        private EmployeeDTO CreateChild(OrgRoleDTO role)
        {
            var empAtRole = (from emp in AllEmployees where emp.RoleId == role.Id select emp).FirstOrDefault() ;
            empAtRole = empAtRole == null ? new EmployeeDTO() { Name = role.Name }: empAtRole;
            if (role.ChildRoles.Count > 0)
            {
                var childRoles = (from subRole in AllRoles where subRole.ParentRoleId == role.Id select subRole).ToList();
                foreach (OrgRoleDTO childRole in childRoles)
                {
                    if (empAtRole.SubOrdinates == null)
                    {
                        empAtRole.SubOrdinates = new List<EmployeeDTO>();
                    }
                    empAtRole.SubOrdinates.Add(CreateChild(childRole));
                }
            }
            return empAtRole;
        }


        private List<OrgRoleDTO> GetAllRoles()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                var response = HttpManager.Instance.Get(string.Empty, APIPaths.RoleGetAll);
                var actionResult = JsonConvert.DeserializeObject<ActionResult<List<OrgRoleDTO>>>(response);
                if (actionResult.ReturnCode == eReturnCode.Success)
                {
                    List<OrgRoleDTO> roles = actionResult.Result;
                    return roles;
                }
                else
                    return null;

                
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return null;
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
            
        }
         
        private List<EmployeeDTO> GetAllActiveEmployees()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                var response = HttpManager.Instance.Get(string.Empty, APIPaths.EmployeeGetAllActive);
                var actionResult = JsonConvert.DeserializeObject<ActionResult<List<EmployeeDTO>>>(response);

                if (actionResult.ReturnCode == eReturnCode.Success)
                {
                    List<EmployeeDTO> employees = actionResult.Result;
                    return employees;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return null;
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
          
        }

        private void InitializeCommands()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            SaveCommand = new RelayCommand(SaveCommandHandler);
            EditCommand = new RelayCommand<int>(EditCommandHandler);
            DeleteCommand = new RelayCommand<int>(DeleteCommandHandler);
            CancelCommand = new RelayCommand(CancelCommandHandler);
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void EditCommandHandler(int obj)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                SelectedEmployee = null;
                SelectedEmployee = (from emp in AllEmployees where emp.Id == obj select emp).FirstOrDefault().Clone() as EmployeeDTO;
                if (SelectedEmployee != null)
                {
                    _isEdit = true;
                    PageTitle = Application.Current.MainWindow.Resources["txtEditEmployee"].ToString();
                    RaisePropertyChanged("PageTitle");

                }
                RaisePropertyChanged("SelectedEmployee");
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

        private void DeleteCommandHandler(int obj)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                SelectedEmployee = (from emp in AllEmployees where emp.Id == obj select emp).FirstOrDefault();
                if (SelectedEmployee != null)
                {
                    Messenger.Default.Register<MsgBxResultMessage>(this, DeleteConfirmResponseHandler);
                    ShowMessageBox(eMessageBoxType.Confirmation, Application.Current.MainWindow.Resources["txtMsgTitleConfirm"].ToString(),
                      Application.Current.MainWindow.Resources["txtMsgTextEmpDeleteConfirm"].ToString());
                }
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

        private void DeleteConfirmResponseHandler(MsgBxResultMessage obj)
        {


            if (obj.Result == eMessageBoxResult.Yes)
            {
                string jsonObj = JsonConvert.SerializeObject(SelectedEmployee);
                var httpContent = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                var response = HttpManager.Instance.Post(httpContent, APIPaths.RoleDelete);
                ActionResult<string> actionResult = JsonConvert.DeserializeObject<ActionResult<string>>(response);

                if (actionResult.ReturnCode == eReturnCode.Success)
                {
                    ShowMessageBox(eMessageBoxType.Info, Application.Current.MainWindow.Resources["txtMsgTitleSuccess"].ToString(),
                     Application.Current.MainWindow.Resources["txtMsgTextEmpDelSuccess"].ToString());
                    FetchData();
                }
                else
                {
                    ShowMessageBox(eMessageBoxType.Error, Application.Current.MainWindow.Resources["txtMsgTitleError"].ToString(),
                      Application.Current.MainWindow.Resources["txtMsgTextEmpDelError"].ToString());
                }
                FetchData();
            }
            

            SelectedEmployee = new EmployeeDTO();
            SelectedEmployee.Manager = new EmployeeDTO();
            SelectedEmployee.EmployeeRole = new OrgRoleDTO();

            Messenger.Default.Unregister<MsgBxResultMessage>(this, DeleteConfirmResponseHandler);
        }

        private void CancelCommandHandler()
        {
            _isEdit = false;
            SelectedEmployee = new EmployeeDTO();
            SelectedEmployee.Manager = new EmployeeDTO();
            PageTitle = Application.Current.MainWindow.Resources["txtAddEmployee"].ToString();
            RaisePropertyChanged("PageTitle");
        }

        private void SaveCommandHandler()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                SelectedEmployee.RoleId = SelectedEmployee.EmployeeRole.Id;
                SelectedEmployee.Manager = null;
                SelectedEmployee.SubOrdinates = null;
                SelectedEmployee.EmployeeRole = null;
                
                if (!_isEdit)
                {
                    //Add Code
                   
                    string jsonObj = JsonConvert.SerializeObject(SelectedEmployee);
                    var httpContent = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    var response = HttpManager.Instance.Post(httpContent, APIPaths.EmployeeCreate);
                    var actionResult = JsonConvert.DeserializeObject<ActionResult<EmployeeDTO>>(response);
                    if (actionResult.ReturnCode == eReturnCode.Success)
                    {
                        ShowMessageBox(eMessageBoxType.Info, Application.Current.MainWindow.Resources["txtMsgTitleSuccess"].ToString(),
                            Application.Current.MainWindow.Resources["txtMsgTextEmpAddSuccess"].ToString());

                    }
                    else
                    {
                        ShowMessageBox(eMessageBoxType.Error, Application.Current.MainWindow.Resources["txtMsgTitleError"].ToString(),
                            Application.Current.MainWindow.Resources["txtMsgTextEmpAddError"].ToString());
                    }

                }
                else
                {
                    //Edit Code
                    string jsonObj = JsonConvert.SerializeObject(SelectedEmployee);
                    var httpContent = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    var response = HttpManager.Instance.Post(httpContent, APIPaths.EmployeeUpdate);
                    var actionResult = JsonConvert.DeserializeObject<ActionResult<EmployeeDTO>>(response);
                    if (actionResult.ReturnCode == eReturnCode.Success)
                    {
                        ShowMessageBox(eMessageBoxType.Info, Application.Current.MainWindow.Resources["txtMsgTitleSuccess"].ToString(),
                            Application.Current.MainWindow.Resources["txtMsgTextEmpEditSuccess"].ToString());

                    }
                    else
                    {
                        ShowMessageBox(eMessageBoxType.Error, Application.Current.MainWindow.Resources["txtMsgTitleError"].ToString(),
                            Application.Current.MainWindow.Resources["txtMsgTextEmpEditError"].ToString());
                    }
                }
                FetchData();

                SelectedEmployee = new EmployeeDTO();

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

        private void ShowMessageBox(eMessageBoxType Type, string title, string text)
        {
            Messenger.Default.Send<ShowMsgBxMessage>(new ShowMsgBxMessage()
            {
                Type = Type,
                Title = title,
                Text = text
            });
        }
    }
}
