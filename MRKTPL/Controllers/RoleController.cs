using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MRKTPL.Data.Entities;
using MRKTPL.Data.ViewModel;
using MRKTPL.LoggerService;
using MRKTPL.Services.RoleServices;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;

namespace MRKTPL.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        #region Fields
        private readonly IRoleServices _roleService;
        //private ILoggerManager _logger;

        #endregion
        #region CTor
        public RoleController(IRoleServices roleServices)//, ILoggerManager logger)
        {
            _roleService = roleServices;
           // _logger = logger;
        }
        #endregion
        #region API's for Create / Edit / Delete
        [HttpGet("{roleID}")]
        public IActionResult GetRoleByID(int roleID)
        {
            try
            {
                //_logger.LogInfo("Fetching all the roles from the storage");
                RoleMaster role = _roleService.GetRolebyId(roleID);

                RoleViewModel roleVM = Mapper.Map<RoleViewModel>(role);

               // _logger.LogInfo($"Returning {role} roles.");

                return Ok(role);
            }
            catch (Exception ex)
            {
                throw ex;
                //_logger.LogError($"Something went wrong: {ex}");
                //return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("CreateRole")]
        public HttpResponseMessage CreateRole([FromBody] RoleViewModel roleVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_roleService.CheckRoleExits(roleVM.RoleName))
                    {
                        HttpResponseMessage response = new HttpResponseMessage()
                        {
                            StatusCode = HttpStatusCode.Conflict
                        };
                        return response;
                    }
                    else
                    {
                        string userId = User.FindFirstValue(ClaimTypes.Name);
                        RoleMaster role = Mapper.Map<RoleMaster>(roleVM);
                        role.IsActive = true;

                        role.CreatedDate = DateTime.Now;
                        role.CreatedBy = Convert.ToInt32(userId);
                        if (role != null)
                        {
                            _roleService.InsertRole(role);
                        }

                        HttpResponseMessage response = new HttpResponseMessage()
                        {
                            StatusCode = HttpStatusCode.OK
                        };
                        return response;
                    }
                }
                else
                {
                    HttpResponseMessage response = new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.BadRequest
                    };
                    return response;
                }
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        [Route("getAllRole")]
        public IActionResult GetAll()
        {
            IEnumerable<RoleMaster> role = _roleService.GetAllRole();
            List<RoleViewModel> roleVM = new List<RoleViewModel>();

            try
            {
                if (role != null)
                {
                    foreach (RoleMaster r in role)
                    {
                        MapperConfiguration config = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<RoleMaster, RoleViewModel>();
                        });
                        IMapper mapper = config.CreateMapper();
                        RoleViewModel dest = Mapper.Map<RoleMaster, RoleViewModel>(r);
                        roleVM.Add(dest);
                    }
                    throw new Exception("Exception while fetching all the students from the storage.");
                }
                //_logger.LogError($"Returning {role} roles.");
                

                

                return Ok(roleVM);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception while fetching all the students from the storage.", ex);
                //throw ex;
            }

        }
        [HttpPut("{id}")]
        public IActionResult UpdateRole(int id, [FromBody] RoleViewModel roleVM)
        {
            try
            {
                RoleMaster role = Mapper.Map<RoleMaster>(roleVM);
                role.RoleID = id;
                _roleService.UpdateRole(role);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public IActionResult EditRole(int id, [FromBody] RoleViewModel roleVM)
        {
            try
            {
                RoleMaster role = null;
                role = Mapper.Map<RoleMaster>(roleVM);
                role = _roleService.GetRolebyId(id);
                role.RoleID = id;
                role.RoleID = roleVM.RoleID;
                role.RoleName = roleVM.RoleName;
                role.RoleDescription = roleVM.RoleDescription;
                role.IsActive = roleVM.IsActive;
                role.CreatedBy = roleVM.CreatedBy;
                role.CreatedDate = roleVM.CreatedDate;

                return Ok(role);
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteRole(int id)
        {
            try
            {
                _roleService.DeleteRole(id);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion
    }
}
