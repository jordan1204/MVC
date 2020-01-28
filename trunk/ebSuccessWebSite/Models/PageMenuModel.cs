using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonResource.Models;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;

namespace ebSuccessWebSite.Models
{
    public class PageMenuModel
    {
        public string Name { get; set; }
        public int UnitId { get; set; }
        public int ConentId { get; set; }
        public int PageId { get; set; }
        public bool Enable { get; set; }
        public List<PageMenuModel> SubMenu { get; set; }
    }
}