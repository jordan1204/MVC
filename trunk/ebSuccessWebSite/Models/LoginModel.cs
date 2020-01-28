using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ebSuccessWebSite.Models
{
    public class LoginModel
    {
        [Display(Name = "帳號")]
        [Required]
        public string Account { get; set; }

        [Display(Name = "密碼")]
        [Required]
        public string Password { get; set; } 

    }
}