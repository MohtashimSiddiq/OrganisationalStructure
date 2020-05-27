using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruag.Data.Models
{
    public class OrgRole
    {
        public int? Id { get; set; }

        public string Name { get; set; }
        public int? ParentRoleId { get; set; }
        [ForeignKey("ParentRoleId")]
        public virtual OrgRole ParentRole { get; set; }
        
        public string Description { get; set; }
        public ICollection<OrgRole> ChildRoles { get; set; }

        public OrgRole()
        {
            ChildRoles = new List<OrgRole>();
        }

    }
}
