﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonResource.Models;
using System.Net.Mail;

namespace ebSuccessWebSite.Models
{
    public class BannerUploadModel
    {          
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Alt { get; set; }
        public HttpPostedFileBase Picture { get; set; }
        public string MimeType { get; set; }
    }
}