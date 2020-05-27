using Ruag.Common;
using Ruag.Data.Models;
using Ruag.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruag.Data.Interfaces
{
    public interface IOrgRoleRepository
    {
        ActionResult<OrgRoleDTO> Add(OrgRoleDTO role);
        ActionResult<string> Update(OrgRoleDTO employee);
        ActionResult<string> Delete(int id);
        ActionResult<OrgRoleDTO>  Get(int Id);
        ActionResult<OrgRoleDTO> Get(string name);
        ActionResult<List<OrgRoleDTO>> GetAll();

    }
}
