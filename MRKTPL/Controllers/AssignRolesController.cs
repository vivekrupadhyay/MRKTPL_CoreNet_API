using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRKTPL.Data.Entities;
using MRKTPL.Data.ViewModel;
using MRKTPL.Services.RoleServices;


namespace MRKTPL.Controllers
{
    
        [Authorize]
        [Route("api/[controller]")]
        [ApiController]
        public class AssignRolesController : ControllerBase
        {
            private readonly IUsersInRoles _usersInRoles;
            public AssignRolesController(IUsersInRoles usersInRoles)
            {
                _usersInRoles = usersInRoles;
            }


            // GET: api/UsersInRoles
            [HttpGet]
            public IEnumerable<AssignRolesViewModel> Get()
            {
                try
                {
                    return _usersInRoles.GetAssignRoles();
                }
                catch (Exception)
                {

                    throw;
                }
            }


            // POST: api/UsersInRoles
            [HttpPost]
            [Route("AssignRole")]
            public HttpResponseMessage Post([FromBody] RoleRightsMaster usersInRoles)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (_usersInRoles.CheckRoleExists(usersInRoles))
                        {
                            var response = new HttpResponseMessage()
                            {
                                StatusCode = HttpStatusCode.Conflict
                            };

                            return response;
                        }
                        else
                        {

                            usersInRoles.RoleRightsMappingID = 0;
                            _usersInRoles.AssignRole(usersInRoles);

                            var response = new HttpResponseMessage()
                            {
                                StatusCode = HttpStatusCode.OK
                            };

                            return response;
                        }
                    }
                    else
                    {
                        var response = new HttpResponseMessage()
                        {

                            StatusCode = HttpStatusCode.BadRequest
                        };

                        return response;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

        [HttpPost]
        [Route("RemoveRole")]
        public HttpResponseMessage RemoveRole([FromBody] RoleRightsMaster usersInRoles)
        {
            if (ModelState.IsValid)
            {
                if (_usersInRoles.CheckRoleExists(usersInRoles))
                {

                    usersInRoles.RoleRightsMappingID = 0;
                    _usersInRoles.RemoveRole(usersInRoles);

                    var response = new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK
                    };

                    return response;
                }
                else
                {
                    var response = new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.Conflict
                    };

                    return response;
                }
            }
            else
            {
                var response = new HttpResponseMessage()
                {

                    StatusCode = HttpStatusCode.BadRequest
                };

                return response;
            }
        }


    }
}
