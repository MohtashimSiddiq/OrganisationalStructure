﻿using GalaSoft.MvvmLight;
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
using Ruag.Common;
using Ruag.Common.Enums;
using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using System.Threading;

namespace Ruag.Client.ViewModel
{
    public class RoleViewModel: ViewModelBase
    {
        public bool GridDisplaySelected { get; set; }
        
        public bool _isEdit = false;
        public string PageTitle { get; set; }
        public OrgRoleDTO _selectedRole;
        public OrgRoleDTO SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                _selectedRole = value;
                RaisePropertyChanged("SelectedRole");
            }
        }
        public ObservableCollection<OrgRoleDTO> AllRoles { get; set; }

        public ObservableCollection<OrgRoleDTO> TreeData { get; set; }

        #region Commands
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand<int> EditCommand { get; set; }
        public RelayCommand<int> DeleteCommand { get; set; }
        #endregion
        public RoleViewModel()
        {
            InitializeCommands();
            
            GridDisplaySelected = true;
            RaisePropertyChanged("GridDisplaySelected");
            PageTitle = Application.Current.MainWindow.Resources["txtAddRole"].ToString();
            RaisePropertyChanged("PageTitle");
            
            SelectedRole = new OrgRoleDTO();
            RaisePropertyChanged("SelectedRole");
            ThreadStart ts = new ThreadStart(FetchData);
            Thread t = new Thread(ts);
            ts.BeginInvoke(null, null);
        }

        private void FetchData()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                AllRoles = new ObservableCollection<OrgRoleDTO>(GetAllRoles());
                ConstructTree();
                SelectedRole.ParentRole = AllRoles.Where(r => r.ParentRoleId == 0).FirstOrDefault().Clone() as OrgRoleDTO;
                RaisePropertyChanged("AllRoles");
                RaisePropertyChanged("TreeData");
                RaisePropertyChanged("SelectedRole");
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

        private void ConstructTree()
        {
            OrgRoleDTO rootNode = (from role in AllRoles where role.ParentRoleId == 0 select role).FirstOrDefault();
            CreateChild(rootNode);
            TreeData = new ObservableCollection<OrgRoleDTO>();
            TreeData.Add(rootNode);
            RaisePropertyChanged("TreeData");
        }


        private OrgRoleDTO CreateChild(OrgRoleDTO role)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {

                if (role.ChildRoles.Count > 0)
                {
                    var childRoles = (from subRole in AllRoles where subRole.ParentRoleId == role.Id select subRole).ToList();
                    foreach (OrgRoleDTO childRole in childRoles)
                    {
                        CreateChild(childRole);
                    }
                    if (childRoles.Count > 0)
                    {
                        role.ChildRoles = childRoles;
                    }

                }
                return role;
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


        private List<OrgRoleDTO> GetAllRoles()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                var response = HttpManager.Instance.Get(string.Empty, APIPaths.RoleGetAll);
                var actionResult = JsonConvert.DeserializeObject<ActionResult<List<OrgRoleDTO>>>(response);
                List<OrgRoleDTO> roles = actionResult.Result;
                return roles;
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
                SelectedRole = null;
                SelectedRole = (from role in AllRoles where role.Id == obj select role).FirstOrDefault().Clone() as OrgRoleDTO;
                if (SelectedRole != null)
                {
                    _isEdit = true;
                    PageTitle = Application.Current.MainWindow.Resources["txtEditRole"].ToString();
                    RaisePropertyChanged("PageTitle");
                    
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

        private void DeleteCommandHandler(int obj)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                SelectedRole = (from role in AllRoles where role.Id == obj select role).FirstOrDefault();
                if (SelectedRole != null)
                {
                   
                    Messenger.Default.Register<MsgBxResultMessage>(this, DeleteConfirmResponseHandler);
                    ShowMessageBox(eMessageBoxType.Confirmation, Application.Current.MainWindow.Resources["txtMsgTitleConfirm"].ToString(),
                           Application.Current.MainWindow.Resources["txtMsgTextRoleDeleteConfirm"].ToString());
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
                string jsonObj = JsonConvert.SerializeObject(SelectedRole);
                var httpContent = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                var response = HttpManager.Instance.Post(httpContent, APIPaths.RoleDelete);
                ActionResult<string> actionResult = JsonConvert.DeserializeObject<ActionResult<string>>(response);

                switch (actionResult.ReturnCode)
                {
                    case eReturnCode.Success:
                        ShowMessageBox(eMessageBoxType.Info, Application.Current.MainWindow.Resources["txtMsgTitleSuccess"].ToString(),
                            Application.Current.MainWindow.Resources["txtMsgTextRoleDelSuccess"].ToString());
                        break;
                    case eReturnCode.Failure:
                        ShowMessageBox(eMessageBoxType.Error, Application.Current.MainWindow.Resources["txtMsgTitleError"].ToString(),
                            Application.Current.MainWindow.Resources["txtMsgTextRoleDelError"].ToString());
                        break;
                    case eReturnCode.EmployeeExists:
                        ShowMessageBox(eMessageBoxType.Error, Application.Current.MainWindow.Resources["txtMsgTitleError"].ToString(),
                           Application.Current.MainWindow.Resources["txtMsgTextRoleDelErrorEmployeeExists"].ToString());
                        break;
                    case eReturnCode.SubOrdinateExists:
                        ShowMessageBox(eMessageBoxType.Error, Application.Current.MainWindow.Resources["txtMsgTitleError"].ToString(),
                           Application.Current.MainWindow.Resources["txtMsgTextRoleDelErrorSubOrdinatesExists"].ToString());
                        break;

                }
                    //else
                    //{
                    //    ShowMessageBox(eMessageBoxType.Error, Application.Current.MainWindow.Resources["txtMsgTitleError"].ToString(),
                    //        Application.Current.MainWindow.Resources["txtMsgTextRoleDelError"].ToString());
                    //}
                    FetchData();
                }
            

            SelectedRole = new OrgRoleDTO();
            SelectedRole.ParentRole = AllRoles.Where(r => r.ParentRoleId == 0).FirstOrDefault().Clone() as OrgRoleDTO;
            RaisePropertyChanged("SelectedRole");

            Messenger.Default.Unregister<MsgBxResultMessage>(this, DeleteConfirmResponseHandler);
        }

        private void CancelCommandHandler()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                _isEdit = false;
                SelectedRole = new OrgRoleDTO();
                SelectedRole.ParentRole = AllRoles.Where(r=>r.ParentRoleId == 0).FirstOrDefault().Clone() as OrgRoleDTO;
                RaisePropertyChanged("SelectedRole");
                PageTitle = Application.Current.MainWindow.Resources["txtAddRole"].ToString();
                RaisePropertyChanged("PageTitle");
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

        private void SaveCommandHandler()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                
                if (!_isEdit)
                {
                    //Add Code
                    SelectedRole.ParentRoleId = SelectedRole.ParentRole.Id;
                    SelectedRole.ParentRole = null;
                    string jsonObj = JsonConvert.SerializeObject(SelectedRole);
                    var httpContent = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    var response = HttpManager.Instance.Post(httpContent, APIPaths.RoleCreate);
                    ActionResult<OrgRoleDTO> actionResult = JsonConvert.DeserializeObject<ActionResult<OrgRoleDTO>>(response);
                    if (actionResult.ReturnCode == eReturnCode.Success)
                    {
                        ShowMessageBox(eMessageBoxType.Info , Application.Current.MainWindow.Resources["txtMsgTitleSuccess"].ToString(),
                            Application.Current.MainWindow.Resources["txtMsgTextRoleAddSuccess"].ToString());
                       
                    }
                    else
                    {
                        ShowMessageBox(eMessageBoxType.Error, Application.Current.MainWindow.Resources["txtMsgTitleError"].ToString(),
                            Application.Current.MainWindow.Resources["txtMsgTextRoleAddError"].ToString());
                    }
                    FetchData();

                    SelectedRole = new OrgRoleDTO();
                    SelectedRole.ParentRole = AllRoles.Where(r => r.ParentRoleId == 0).FirstOrDefault().Clone() as OrgRoleDTO;
                    RaisePropertyChanged("SelectedRole");
                }
                else
                {
                    if (SelectedRole.Id == SelectedRole.ParentRole.Id)
                    {
                        ShowMessageBox(eMessageBoxType.Error, Application.Current.MainWindow.Resources["txtMsgTitleError"].ToString(),
                                                    Application.Current.MainWindow.Resources["txtMsgTextParentRoleModifiedToSelf"].ToString());
                        return;
                    }
                    
                    if (!ValidateRoleEdit(SelectedRole.ParentRole))
                    {
                        ShowMessageBox(eMessageBoxType.Error, Application.Current.MainWindow.Resources["txtMsgTitleError"].ToString(),
                            Application.Current.MainWindow.Resources["txtMsgTextParentRoleModifiedToSelf"].ToString());
                        return;
                    }
                    Messenger.Default.Register<MsgBxResultMessage>(this, EditConfirmResponseHandler);
                    ShowMessageBox(eMessageBoxType.Confirmation, Application.Current.MainWindow.Resources["txtMsgTitleConfirm"].ToString(),
                           Application.Current.MainWindow.Resources["txtMsgTextRoleEditConfirm"].ToString());
                    return;

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


        private bool ValidateRoleEdit(OrgRoleDTO role)
        {
            bool returnVal = true;
            if (role.Id == SelectedRole.Id)
            {
                return false;
                
            }
            if (role.ParentRole != null)
            {
                returnVal = returnVal == true ? ValidateRoleEdit(role.ParentRole) : returnVal;
            }            
            return returnVal;
        }

        private void EditConfirmResponseHandler(MsgBxResultMessage obj)
        {
            if (obj.Result == eMessageBoxResult.Yes)
            {
                //Edit Code
                SelectedRole.ParentRoleId = SelectedRole.ParentRole.Id;
                SelectedRole.ParentRole = null;
                string jsonObj = JsonConvert.SerializeObject(SelectedRole);
                var httpContent = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                var response = HttpManager.Instance.Post(httpContent, APIPaths.RoleUpdate);
                ActionResult<OrgRoleDTO> actionResult = JsonConvert.DeserializeObject<ActionResult<OrgRoleDTO>>(response);
                if (actionResult.ReturnCode == eReturnCode.Success)
                {
                    ShowMessageBox(eMessageBoxType.Info, Application.Current.MainWindow.Resources["txtMsgTitleSuccess"].ToString(),
                        Application.Current.MainWindow.Resources["txtMsgTextRoleEditSuccess"].ToString());

                }
                else
                {
                    ShowMessageBox(eMessageBoxType.Error, Application.Current.MainWindow.Resources["txtMsgTitleError"].ToString(),
                        Application.Current.MainWindow.Resources["txtMsgTextRoleEditError"].ToString());
                }
                FetchData();
                SelectedRole = new OrgRoleDTO();
                SelectedRole.ParentRole = AllRoles.Where(r => r.ParentRoleId == 0).FirstOrDefault().Clone() as OrgRoleDTO;
                RaisePropertyChanged("SelectedRole");
            }


            SelectedRole = new OrgRoleDTO();
            SelectedRole.ParentRole = AllRoles.Where(r => r.ParentRoleId == 0).FirstOrDefault().Clone() as OrgRoleDTO;
            RaisePropertyChanged("SelectedRole");
            Messenger.Default.Unregister<MsgBxResultMessage>(this, DeleteConfirmResponseHandler);
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
