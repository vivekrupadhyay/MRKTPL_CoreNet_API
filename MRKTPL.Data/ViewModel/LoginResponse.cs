using System;
using System.Collections.Generic;
using System.Text;

namespace MRKTPL.Data.ViewModel
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public int RoleId { get; set; }
    }

}
