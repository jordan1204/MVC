using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonResource.Models;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;

namespace ebSuccessWebSite.Models
{
    public class PageAdminModel
    {
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string HtmlContent { get; set; }
        public bool Enable { get; set; }
        public byte[] PictureData { get; set; }
        public HttpPostedFileBase Picture { get; set; }
        public string MimeType { get; set; }
    }
}