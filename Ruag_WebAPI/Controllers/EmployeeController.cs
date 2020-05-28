using Ruag.Common;
using Ruag.Data;
using Ruag.Data.Interfaces;
using Ruag.Data.Repository;
using Ruag.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;

namespace Ruag_WebAPI.Controllers
{
    //[Authorize]
    [RoutePrefix("api/employee")]
    public class EmployeeController : BaseApiController
    {
        IEmployeeRepository iEmployeeRepository;
        [HttpPost]
        [AllowAnonymous]
        [Route("Create")]
        public IHttpActionResult Create(EmployeeDTO employeeDTO)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            iEmployeeRepository = new EmployeeRepository(this.Request.GetOwinContext().Get<AppDBContext>());
            ActionResult<EmployeeDTO> actionResult = iEmployeeRepository.Add(employeeDTO);
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return Ok(actionResult);
           
        }


        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update(EmployeeDTO empDTO)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            iEmployeeRepository = new EmployeeRepository(this.Request.GetOwinContext().Get<AppDBContext>());
            ActionResult<string> actionResult = iEmployeeRepository.Update(empDTO);
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return Ok(actionResult);

        }

        [HttpPost]
        [Route("Delete")]
        public IHttpActionResult Delete(EmployeeDTO emp)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            iEmployeeRepository = new EmployeeRepository(this.Request.GetOwinContext().Get<AppDBContext>());
            ActionResult<string> actionResult = iEmployeeRepository.Delete(emp.Id);
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return Ok(actionResult);

        }


        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            iEmployeeRepository = new EmployeeRepository(this.Request.GetOwinContext().Get<AppDBContext>());
            ActionResult<EmployeeDTO> actionResult = iEmployeeRepository.Get(id);
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return Ok(actionResult);

        }

        [HttpGet]
        [Route("Get/Name")]
        public IHttpActionResult GetByName(string name)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            iEmployeeRepository = new EmployeeRepository(this.Request.GetOwinContext().Get<AppDBContext>());
            ActionResult<EmployeeDTO> actionResult = iEmployeeRepository.Get(name);
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return Ok(actionResult);

        }

        [HttpGet]
        [Route("GetAllActive")]
        public IHttpActionResult GetAllActive()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            iEmployeeRepository = new EmployeeRepository(this.Request.GetOwinContext().Get<AppDBContext>());
            ActionResult<List<EmployeeDTO>> actionResult = iEmployeeRepository.GetAllActive();
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return Ok(actionResult);

        }

        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            iEmployeeRepository = new EmployeeRepository(this.Request.GetOwinContext().Get<AppDBContext>());
            ActionResult<List<EmployeeDTO>> actionResult = iEmployeeRepository.GetAll();
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return Ok(actionResult);

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("test")]
        public IHttpActionResult Test()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            iEmployeeRepository = new EmployeeRepository(this.Request.GetOwinContext().Get<AppDBContext>());
            ActionResult<EmployeeDTO> actionResult = iEmployeeRepository.Get(1);
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return Ok(actionResult);

        }


    }
}
