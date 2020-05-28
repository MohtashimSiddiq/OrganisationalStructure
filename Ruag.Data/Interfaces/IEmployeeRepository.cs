using Ruag.Common;
using Ruag.Data.Models;
using Ruag.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruag.Data.Interfaces
{
    public interface IEmployeeRepository
    {
        ActionResult<EmployeeDTO> Add(EmployeeDTO employee);
        ActionResult<string> Update(EmployeeDTO employee);
        ActionResult<string> Delete(int employeeId);
        ActionResult<EmployeeDTO> Get(int Id);
        ActionResult<EmployeeDTO> Get(string name);
        ActionResult<List<EmployeeDTO>>  GetAll();
        ActionResult<List<EmployeeDTO>> GetAllActive();
    }
}
