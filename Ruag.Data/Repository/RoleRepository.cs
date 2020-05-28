using Ruag.Common;
using Ruag.Common.Enums;
using Ruag.Data.Interfaces;
using Ruag.Data.Models;
using Ruag.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruag.Data.Repository
{
    public class OrgRoleRepository : IOrgRoleRepository
    {
        private AppDBContext _appDBContext;
        private DbSet<OrgRole> _orgRoles;
        private List<OrgRole> _orgChildRoles;
        private DbSet<Employee> _employees;
        public OrgRoleRepository(AppDBContext dBContext)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            _appDBContext = dBContext;
            _orgRoles = dBContext.OrgRoles;
            _employees = dBContext.Employees;
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
        public ActionResult<OrgRoleDTO> Add(OrgRoleDTO roleDTO)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                OrgRole orgRoleModel = ModelFactory.Instance.CreateModel(roleDTO);
                _orgRoles.Add(orgRoleModel);
                _appDBContext.SaveChanges();
                return new ActionResult<OrgRoleDTO>()
                {
                    ReturnCode = eReturnCode.Success,
                    ReturnDescription = "Role added successfully",
                    Result = ModelFactory.Instance.CreateDTO(orgRoleModel)
                };
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return new ActionResult<OrgRoleDTO>() { ReturnCode = eReturnCode.Failure, ReturnDescription = "Role add error" };
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
        }

        public ActionResult<string> Delete(int id)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                OrgRole role = _orgRoles.Where(r => r.Id == id).Include("ChildRoles").FirstOrDefault();
                if (role != null)
                {
                    var emp = (from employee in _employees where employee.RoleId == role.Id select employee).FirstOrDefault();
                    if (emp == null)
                    {
                        _orgChildRoles = new List<OrgRole>();
                        GetChildRolesToDelete(role);
                        List<int> existingEmployeeRoles = _employees.Select(e => e.RoleId.Value).ToList();
                        int employeesInChildRolesCount =_orgChildRoles.Where(r => r.Id.Value.In(existingEmployeeRoles)).Count();
                        if (employeesInChildRolesCount > 0)
                        {
                            return new ActionResult<string>() { ReturnCode = eReturnCode.SubOrdinateExists, ReturnDescription = "Role deleted error. Subordiates exists", Result = "" };
                        }
                        else
                        {
                            foreach (OrgRole orgRole in _orgChildRoles)
                            {
                                _orgRoles.Remove(orgRole);
                            }
                        }
                    }
                    else
                    {
                        return new ActionResult<string>() { ReturnCode = eReturnCode.EmployeeExists, ReturnDescription = "Role delete error.Employee exists.", Result = "" };
                    }
                    
                }
                _appDBContext.SaveChanges();
                return new ActionResult<string>() { ReturnCode = eReturnCode.Success, ReturnDescription = "Role deleted successfully", Result = "" };
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return new ActionResult<string>() { ReturnCode = eReturnCode.Failure, ReturnDescription = "Role delete error", Result = "" };
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
        }

        private void GetChildRolesToDelete(OrgRole childRole)
        {
            OrgRole tempRole = _orgRoles.Where(r => r.Id == childRole.Id).Include("ChildRoles").FirstOrDefault();

            foreach (OrgRole childrole in tempRole.ChildRoles)
            {
                GetChildRolesToDelete(childrole);
            }
            _orgChildRoles.Add(tempRole);
        }

        public ActionResult<OrgRoleDTO> Get(int id)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                OrgRole role = _orgRoles.Where(e => e.Id == id).Include("ChildRoles").FirstOrDefault();
                OrgRoleDTO roleDTO = null;
                if (role != null)
                {
                    roleDTO = ModelFactory.Instance.CreateDTO(role);
                }
                return new ActionResult<OrgRoleDTO>() { ReturnCode = eReturnCode.Success, ReturnDescription = "Role get success", Result = roleDTO };
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return new ActionResult<OrgRoleDTO>() { ReturnCode = eReturnCode.Failure, ReturnDescription = "Role get failure" };
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
        }

        public ActionResult<OrgRoleDTO> Get(string name)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                OrgRole role = _orgRoles.Where(e => e.Name == name).FirstOrDefault();
                OrgRoleDTO roleDTO = null;
                if (role != null)
                {
                    roleDTO = ModelFactory.Instance.CreateDTO(role);
                }
                return new ActionResult<OrgRoleDTO>() { ReturnCode = eReturnCode.Success, ReturnDescription = "Role get success", Result = roleDTO };
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return new ActionResult<OrgRoleDTO>() { ReturnCode = eReturnCode.Success, ReturnDescription = "Role get error" };
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
        }

        public ActionResult<List<OrgRoleDTO>> GetAll()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                List<OrgRole> orgRoleList = _orgRoles.Include("ParentRole").Include("ChildRoles").OrderBy(e=>e.ParentRoleId).ToList();
                return new ActionResult<List<OrgRoleDTO>>() { ReturnCode = eReturnCode.Success, ReturnDescription = "returning result", Result = ModelFactory.Instance.CreateDTO(orgRoleList,true) };
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return new ActionResult<List<OrgRoleDTO>>() { ReturnCode = eReturnCode.Failure, ReturnDescription = "returning result Error" };
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
        }

        public ActionResult<string> Update(OrgRoleDTO orgRoleDTO)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                OrgRole orgRole = _orgRoles.Where(e => e.Id == orgRoleDTO.Id).FirstOrDefault();

                if (orgRole != null)
                {
                    orgRole.Name = orgRoleDTO.Name;
                    orgRole.Description = orgRoleDTO.Description;
                    orgRole.ParentRoleId = orgRoleDTO.ParentRoleId;
                    _appDBContext.SaveChanges();
                }
                return new ActionResult<string>() { ReturnCode = eReturnCode.Success, ReturnDescription = "Role updated successfully", Result = "" };
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return new ActionResult<string>() { ReturnCode = eReturnCode.Failure, ReturnDescription = "Role updated successfully", Result = "" };
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
        }
    }
}
