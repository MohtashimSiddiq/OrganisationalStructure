using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruag.DTO
{
    public class OrgRoleDTO:INotifyPropertyChanged,ICloneable
    {

        private int _id { get; set; }

        private string _name { get; set; }
        private int _parentRoleId { get; set; }
        private OrgRoleDTO _parentRole { get; set; }
        private string _description { get; set; }
        private List<OrgRoleDTO> _childRoles { get; set; }




        public int Id { get { return _id; } set { _id = value; RaisePorpertyChanged("Id");  } }

        public string Name { get { return _name; } set { _name = value; RaisePorpertyChanged("Name"); } }
        public int ParentRoleId { get { return _parentRoleId; } set { _parentRoleId = value; RaisePorpertyChanged("ParentRoleId"); } }
        public OrgRoleDTO ParentRole { get { return _parentRole; } set { _parentRole = value; RaisePorpertyChanged("ParentRole"); } }
        public string Description { get { return _description; } set { _description = value; RaisePorpertyChanged("Description"); } }
        public List<OrgRoleDTO> ChildRoles { get { return _childRoles; } set { _childRoles = value; RaisePorpertyChanged("ChildRoles"); } }
        public bool ShowSeperator
        {
            get
            {
                if (ChildRoles != null && ChildRoles.Count > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            set
            {

            }
        }
        public OrgRoleDTO()
        {
            ChildRoles = new List<OrgRoleDTO>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePorpertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
              PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public object Clone()
        {
            OrgRoleDTO role = new OrgRoleDTO();
            role.Id = this.Id;
            role.Name = this.Name.Clone() as string;
            role.ParentRoleId = this.ParentRoleId;
            if (this.ParentRole != null)
            {
                role.ParentRole = this.ParentRole.Clone() as OrgRoleDTO;
            }

            role.Description = this.Name.Clone() as string;
            if (this.ChildRoles != null)
            {
                foreach (OrgRoleDTO childrole in this.ChildRoles)
                {
                    role.ChildRoles.Add(childrole.Clone() as OrgRoleDTO);
                }
            }
            
            
            return role;
        }
    }
}
