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
    [AllowAnonymous]
    [RoutePrefix("api/OrgRole")]
    public class OrgRoleController : ApiController
    {

        IOrgRoleRepository iRoleRepository;

        [HttpPost]       
        [Route("Create")]
        public IHttpActionResult Create(OrgRoleDTO roleDTO)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            iRoleRepository = new OrgRoleRepository(this.Request.GetOwinContext().Get<AppDBContext>());
            ActionResult<OrgRoleDTO> actionResult = iRoleRepository.Add(roleDTO);
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return Ok(actionResult);

        }

        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update(OrgRoleDTO roleDTO)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            iRoleRepository = new OrgRoleRepository(this.Request.GetOwinContext().Get<AppDBContext>());
            ActionResult<string> actionResult = iRoleRepository.Update(roleDTO);
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return Ok(actionResult);

        }

        [HttpPost]
        [Route("Delete")]
        public IHttpActionResult Delete(OrgRoleDTO roleDTO)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            iRoleRepository = new OrgRoleRepository(this.Request.GetOwinContext().Get<AppDBContext>());
            ActionResult<string> actionResult = iRoleRepository.Delete(roleDTO.Id);
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return Ok(actionResult);

        }


        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            iRoleRepository = new OrgRoleRepository(this.Request.GetOwinContext().Get<AppDBContext>());
            ActionResult<OrgRoleDTO> actionResult = iRoleRepository.Get(id);
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return Ok(actionResult);

        }

        [HttpGet]
        [Route("Get/Name")]
        public IHttpActionResult GetByName(string name)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            iRoleRepository = new OrgRoleRepository(this.Request.GetOwinContext().Get<AppDBContext>());
            ActionResult<OrgRoleDTO> actionResult = iRoleRepository.Get(name);
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return Ok(actionResult);

        }

        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            iRoleRepository = new OrgRoleRepository(this.Request.GetOwinContext().Get<AppDBContext>());
            ActionResult<List<OrgRoleDTO>> actionResult = iRoleRepository.GetAll();
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            return Ok(actionResult);

        }




    }
}
