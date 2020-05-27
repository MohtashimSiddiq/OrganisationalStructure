using Ruag.Data.Models;
using Ruag.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruag.Data.Interfaces
{
    public interface IModelFactory
    {
        EmployeeDTO CreateDTO(Employee employee);
        OrgRoleDTO CreateDTO(OrgRole orgRole);
        Employee CreateModel(EmployeeDTO employeeDTO);
        OrgRole CreateModel(OrgRoleDTO orgRole);
    }
}
