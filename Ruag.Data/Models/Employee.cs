using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruag.Data.Models
{
    public class Employee
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
               
        public int? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public OrgRole EmployeeRole { get; set; }

        public int? ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public virtual Employee Manager { get; set; }
        public ICollection<Employee> SubOrdinates { get; set; }
        public bool IsDeleted { get; set; }
        
        public Employee()
        {
            SubOrdinates = new List<Employee>();
        }
    }
}
