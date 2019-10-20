using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MRKTPL.Data.ViewModel
{
   public class LoginRequestViewModel
    {
        [Required(ErrorMessage = "Enter UserName")]
        //public string UserName { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }
        public string Token { get; set; }
        public int Usertype { get; set; }
    }
}
