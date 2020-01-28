using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace ebSuccessWebSite.Models
{
    public class FreeTrialModel
    {
        [Display(Name = "姓名或公司名稱")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "聯絡電話")]
        [Required]
        public string Telephone { get; set; }

        [Display(Name = "行動電話")]
        public string Cellphone { get; set; }

        [DataType(DataType.EmailAddress)] 
        [Display(Name = "電子郵件")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "留言")]
        public string Message { get; set; }

        [Display(Name = "產品/服務名稱")]
        [Required]
        public string ProductName { get; set; }

        public List<SelectListItem> AvailableProduct { get; set; }
    }
}