using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruag.DTO
{
    public class EmployeeDTO : INotifyPropertyChanged, ICloneable
    {
        private int _id;
        private string _name;
        private int _roleId;
        private OrgRoleDTO _employeeRole;

        private int _managerId;
        private EmployeeDTO _manager;
        private List<EmployeeDTO> _subOrdinates;
        private bool _isDeleted;

        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("Id"); }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged("Name"); }
        }
        public int RoleId
        {
            get { return _roleId; }
            set { _roleId = value; RaisePropertyChanged("RoleId"); }
        }
        public OrgRoleDTO EmployeeRole
        {
            get { return _employeeRole; }
            set { _employeeRole = value; RaisePropertyChanged("EmployeeRole"); }
        }

        public int ManagerId
        {
            get { return _managerId; }
            set { _managerId = value; RaisePropertyChanged("ManagerId"); }
        }
        public virtual EmployeeDTO Manager
        {
            get
            {
                return _manager;
            }
            set { _manager = value; RaisePropertyChanged("Manager"); }
        }
        public List<EmployeeDTO> SubOrdinates
        {
            get { return _subOrdinates; }
            set { _subOrdinates = value; RaisePropertyChanged("SubOrdinates"); }
        }
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; RaisePropertyChanged("IsDeleted"); }
        }

        public EmployeeDTO()
        {
            _subOrdinates = new List<EmployeeDTO>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }            
        }

        public object Clone()
        {
            EmployeeDTO emp = new EmployeeDTO();
            emp.Name = this.Name.Clone() as string;
            emp.Id = this.Id;
            emp.RoleId = this.RoleId;
            emp.EmployeeRole = this.EmployeeRole.Clone() as OrgRoleDTO;
            emp.ManagerId = this.ManagerId;
            emp.Manager = this.Manager != null ? this.Manager.Clone() as EmployeeDTO : null;
            if (this.SubOrdinates != null)
            {
                foreach (EmployeeDTO subs in this.SubOrdinates)
                {
                    emp.SubOrdinates.Add(subs.Clone() as EmployeeDTO);
                }
            }
            return emp;
        }
    }
}
