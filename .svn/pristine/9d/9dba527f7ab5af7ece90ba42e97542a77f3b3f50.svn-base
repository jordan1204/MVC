using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace ebSuccessWebSite.Models
{
    public class PageModel
    {       
        public List<Breadcrumb> BreadcrumbList { get; set; }
        public string PageTitle { get; set; }
        public string MetaKeywords { get; set; }   
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string PageContent { get; set; }

        public partial class Breadcrumb
        {
            public string Name { get; set; }
            public int UnitId { get; set; }
            public int? ContentId { get; set; }
            public int? PageId { get; set; }
        }
    }
}