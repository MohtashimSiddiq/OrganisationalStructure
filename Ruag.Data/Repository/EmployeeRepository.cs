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
    public class EmployeeRepository : IEmployeeRepository
    {
        private AppDBContext _appDBContext;
        private DbSet<Employee> _employees;
        private DbSet<OrgRoleDTO> _roles;
        public EmployeeRepository(AppDBContext dBContext)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            _appDBContext = dBContext;
            employees = dBContext.Employees;
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
        public ActionResult<EmployeeDTO> Add(EmployeeDTO empDTO)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                Employee empModel = ModelFactory.Instance.CreateModel(empDTO);
                employees.Add(empModel);
                _appDBContext.SaveChanges();
                return new ActionResult<EmployeeDTO>() { ReturnCode = eReturnCode.Success, ReturnDescription = "Employee Added successfully", Result = ModelFactory.Instance.CreateDTO(empModel) };
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return new ActionResult<EmployeeDTO>() { ReturnCode = eReturnCode.Failure, ReturnDescription = "Employee add failure" };
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
           
        }

        public ActionResult<string> Delete(int empId)
        {

            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                Employee emp = employees.Where(e => e.Id == empId).FirstOrDefault();
                if (emp != null)
                {
                    emp.IsDeleted = true;
                }

                _appDBContext.SaveChanges();
                return new ActionResult<string>() { ReturnCode = eReturnCode.Success, ReturnDescription = "Employee deleted successfully", Result = "" };
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return new ActionResult<string>() { ReturnCode = eReturnCode.Failure, ReturnDescription = "Employee delete failure", Result = ex.ToString() };
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
          
        }

        public ActionResult<EmployeeDTO> Get(int empId)
        {

            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                Employee emp = employees.Where(e => e.Id == empId).Include("EmployeeRole").Include("Manager").Include("SubOrdinates").FirstOrDefault();
                EmployeeDTO empDTO = null;
                if (emp != null)
                {
                    empDTO = ModelFactory.Instance.CreateDTO(emp);
                }
                return new ActionResult<EmployeeDTO>() { ReturnCode = eReturnCode.Success, ReturnDescription = "Employee get successfully", Result = empDTO };
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return new ActionResult<EmployeeDTO>() { ReturnCode = eReturnCode.Failure, ReturnDescription = "Employee get failure"};
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
            
        }

        public ActionResult<EmployeeDTO> Get(string name)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                Employee emp = employees.Where(e => e.Name == name).Include("EmployeeRole").Include("Manager").Include("SubOrdinates").FirstOrDefault();
                EmployeeDTO empDTO = null;
                if (emp != null)
                {
                    empDTO = ModelFactory.Instance.CreateDTO(emp);
                }
                return new ActionResult<EmployeeDTO>() { ReturnCode = eReturnCode.Success, ReturnDescription = "Employee deleted successfully", Result = empDTO };
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return new ActionResult<EmployeeDTO>() { ReturnCode = eReturnCode.Failure, ReturnDescription = "Employee get error" };
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
           
        }

        public ActionResult<List<EmployeeDTO>> GetAll()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                List<Employee> empList = employees.Include("EmployeeRole").ToList();
                foreach(EmployeeDTO emp in empList)
                {

                    emp.
                }
                return new ActionResult<List<EmployeeDTO>>() { ReturnCode = eReturnCode.Success, ReturnDescription = "returning result", Result = ModelFactory.Instance.CreateDTO(empList) };
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return new ActionResult<List<EmployeeDTO>>() { ReturnCode = eReturnCode.Failure, ReturnDescription = "returning result Error" };
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
           
        }

        public ActionResult<string> Update(EmployeeDTO empDTO)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                Employee emp = employees.Where(e => e.Id == empDTO.Id).FirstOrDefault();

                if (emp != null)
                {
                    emp.ManagerId = empDTO.ManagerId;
                    emp.Name = empDTO.Name;
                    emp.RoleId = empDTO.RoleId;
                    _appDBContext.SaveChanges();
                }
                return new ActionResult<string>() { ReturnCode = eReturnCode.Success, ReturnDescription = "Employee updated successfully", Result = "" };
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return new ActionResult<string>() { ReturnCode = eReturnCode.Failure, ReturnDescription = "Employee updated successfully", Result = "" };
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
            
        }
    }
}
