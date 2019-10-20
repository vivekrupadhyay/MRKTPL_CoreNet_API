using System;
using System.Collections.Generic;
using System.Text;

namespace MRKTPL.Data.ViewModel
{
   public class AssignRolesViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public int UserRolesId { get; set; }
    }
}
