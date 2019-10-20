using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MRKTPL.Common;
using MRKTPL.Data.Helper;
using MRKTPL.Data.ViewModel;
using MRKTPL.Services.UserServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MRKTPL.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticateController : Controller
    {
        #region Fields
        private readonly AppSettings _appSettings;
        private readonly IUserServices _users;
        #endregion
        #region CTor
        public AuthenticateController(IOptions<AppSettings> appSettings, IUserServices users)
        {
            _users = users;
            _appSettings = appSettings.Value;
        }
        #endregion
        #region API's
        [HttpPost]
        //[Route("LoginPost")]
        public IActionResult Post([FromBody] LoginRequestViewModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // var a = EncryptionLibrary.EncryptText(value.Password);
                   // var b = EncryptionLibrary.DecryptText("tttdoybuFsAnWJYAfwOUqg==");
                    var loginstatus = _users.AuthenticateUsers(value.Email, EncryptionLibrary.EncryptText(value.Password));

                    if (loginstatus)
                    {
                        var userdetails = _users.GetUserDetailsbyCredentials(value.Email);

                        if (userdetails != null)
                        {

                            var tokenHandler = new JwtSecurityTokenHandler();
                            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[]
                                {
                                        new Claim(ClaimTypes.Name, userdetails.UserId.ToString())
                                }),
                                Expires = DateTime.UtcNow.AddDays(1),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                    SecurityAlgorithms.HmacSha256Signature)
                            };
                            var token = tokenHandler.CreateToken(tokenDescriptor);
                            value.Token = tokenHandler.WriteToken(token);

                            // remove password before returning
                            value.Password = null;
                            value.Usertype = userdetails.RoleId;

                            return Ok(value);

                        }
                        else
                        {
                            value.Password = null;
                            value.Usertype = 0;
                            return Ok(value);
                        }
                    }
                    value.Password = null;
                    value.Usertype = 0;
                    return Ok(value);
                }
                value.Password = null;
                value.Usertype = 0;
                return Ok(value);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
