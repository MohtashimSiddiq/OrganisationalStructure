using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruag.Client.Helpers
{
    public static class APIPaths
    {
        public static string OAuth = "oauth/token";

        public static string RoleCreate = "api/OrgRole/Create";
        public static string RoleUpdate = "api/OrgRole/Update";
        public static string RoleDelete = "api/OrgRole/Delete";
        public static string RoleGetById = "api/OrgRole/GetById";
        public static string RoleGetByName = "api/OrgRole/GetByName";
        public static string RoleGetAll = "api/OrgRole/GetAll";

        public static string EmployeeCreate = "api/employee/create";
        public static string EmployeeUpdate = "api/Employee/update";
        public static string EmployeeDelete = "api/Employee/delete";
        public static string EmployeeGetById = "api/Employee/GetById";
        public static string EmployeeGetByName = "api/Employee/GetByName";
        public static string EmployeeGetAll = "api/Employee/GetAll";
        public static string EmployeeGetAllActive = "api/Employee/GetAllActive";

    }
}
