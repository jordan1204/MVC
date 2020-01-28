using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonResource.Models;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;

namespace ebSuccessWebSite.Models
{
    public class AdminModel
    {
        [Display(Name = "Meta Keywords")]
        public string MetaKeywords { get; set; }
        [Display(Name = "Meta Description")]
        public string MetaDescription { get; set; }
        [Display(Name = "Meta Title")]
        public string MetaTitle { get; set; }
        public string Content { get; set; }
        public string PageName { get; set; }
    }
}