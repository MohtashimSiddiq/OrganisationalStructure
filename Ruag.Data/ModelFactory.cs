using Ruag.Data.Interfaces;
using Ruag.Data.Models;
using Ruag.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ruag.Data
{
    public class ModelFactory 
    {
        private static ModelFactory _instance;

        public static ModelFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ModelFactory();
                }
                return _instance;
            }
        }

        private ModelFactory()
        {

        }
        public EmployeeDTO CreateDTO(Employee employee,bool createChild = true)
        {
            return new EmployeeDTO()
            {
                Id = employee.Id.Value,
                EmployeeRole = employee.EmployeeRole != null? CreateDTO(employee.EmployeeRole):null,
                Name = employee.Name,
                IsDeleted = employee.IsDeleted,
                Manager = employee.Manager != null ? CreateDTO(employee.Manager,false):null,
                ManagerId = employee.ManagerId.Value,
                RoleId = employee.RoleId.Value,
                SubOrdinates = createChild  ? employee.SubOrdinates.ToList().Select(sub => CreateDTO(sub)).ToList():null
            };
        }

        public List<EmployeeDTO> CreateDTO(List<Employee> employees)
        {
            List<EmployeeDTO> empDTOList = new List<EmployeeDTO>();
            foreach (Employee emp in employees)
            {
                empDTOList.Add( CreateDTO(emp,false));
            }
            return empDTOList;
        }



        public OrgRoleDTO CreateDTO(OrgRole orgRole,bool createChild = true)
        {
            return new OrgRoleDTO()
            {
                Id = orgRole.Id.Value,
                Description = orgRole.Description,
                Name = orgRole.Name,
                ParentRole = orgRole.ParentRole == null ? null : CreateDTO(orgRole.ParentRole, false),
                ParentRoleId = orgRole.ParentRoleId.Value,
                ChildRoles = createChild  ? orgRole.ChildRoles.ToList().Select(sub => CreateDTO(sub, true)).ToList() : null

            };
        }

        public List<OrgRoleDTO> CreateDTO(List<OrgRole> orgRoles,bool createWithChild = false)
        {
            List<OrgRoleDTO> orgRoleDTOList = new List<OrgRoleDTO>();
            foreach (OrgRole orgRole in orgRoles)
            {
                orgRoleDTOList.Add(CreateDTO(orgRole, createWithChild));
            }
            return orgRoleDTOList;
        }

        public Employee CreateModel(EmployeeDTO employeeDTO)
        {
            return new Employee()
            {
                Id = employeeDTO.Id,
                EmployeeRole = employeeDTO.EmployeeRole != null? CreateModel(employeeDTO.EmployeeRole):null,
                Name = employeeDTO.Name,
                IsDeleted = employeeDTO.IsDeleted,
                Manager = employeeDTO.Manager != null? CreateModel(employeeDTO.Manager):null,
                ManagerId = employeeDTO.ManagerId,
                RoleId = employeeDTO.RoleId,
                SubOrdinates = employeeDTO.SubOrdinates.ToList().Select(sub => CreateModel(sub)).ToList()
            };
        }

        public OrgRole CreateModel(OrgRoleDTO orgRoleDTO)
        {
            return new OrgRole()
            {
                Id = orgRoleDTO.Id,
                Description = orgRoleDTO.Description,
                Name = orgRoleDTO.Name,
                ParentRole = orgRoleDTO.ParentRole == null ? null: CreateModel(orgRoleDTO.ParentRole),
                ParentRoleId = orgRoleDTO.ParentRoleId,
                ChildRoles = orgRoleDTO.ChildRoles.ToList().Select(sub => CreateModel(sub)).ToList()

            };
        }
    }
}