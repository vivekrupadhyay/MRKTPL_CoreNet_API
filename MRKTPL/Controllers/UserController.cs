using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MRKTPL.Common;
using MRKTPL.Data.Entities;
using MRKTPL.Data.ViewModel;
using MRKTPL.LoggerService;
using MRKTPL.Services.UserServices;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;

namespace MRKTPL.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class UserController : ControllerBase
    {
        #region Fields
        private readonly IUserServices _users;
        private readonly IMapper _mapper;
        //private readonly IErrorLoggerManager _logger;
        #endregion
        #region CTor
        public UserController(IUserServices userServices, IMapper mapper)//, ErrorLoggerManager logger)//, IMapper mapper)
        {
            _users = userServices;
            _mapper = mapper;
            //_logger = logger;
        }
        #endregion
        // GET api/values
        [HttpGet]
        [Route("getall")]
        public IEnumerable<UserMaster> Get()
        {
            return _users.GetAllUsers();
             
          //  return Ok(_mapper.Map<IEnumerable<UserDto>>(result));
        }

        [HttpGet("{id}", Name = "GetUsers")]
        public UserMaster Get(int id)
        {
            return _users.GetUsersbyId(id);
        }
        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Post([FromBody] UserViewModel users)
        {
            if (ModelState.IsValid)
            {
                if (_users.CheckEmailExits(users.Email))
                {
                    HttpResponseMessage response = new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.Conflict
                    };

                    return response;
                }
                else
                {
                    if (users != null)
                    {
                        string userId = User.FindFirstValue(ClaimTypes.Name);
                        UserMaster tempUsers = Mapper.Map<UserMaster>(users);
                        tempUsers.IsActive = true;
                        tempUsers.CreatedDate = DateTime.Now;
                        tempUsers.CreatedBy = Convert.ToInt32(userId);
                        tempUsers.Password = EncryptionLibrary.EncryptText(users.Password);
                        _users.InsertUsers(tempUsers);
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

        [HttpPut("{id}")]
        public HttpResponseMessage Put(int id, [FromBody] UserViewModel users)
        {
            if (ModelState.IsValid)
            {

                UserMaster tempUsers = Mapper.Map<UserMaster>(users);
                tempUsers.CreatedDate = DateTime.Now;
                _users.UpdateUsers(tempUsers);

                HttpResponseMessage response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };

                return response;

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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            bool result = _users.DeleteUsers(id);

            if (result)
            {
                HttpResponseMessage response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };
                return response;
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
    }
}
