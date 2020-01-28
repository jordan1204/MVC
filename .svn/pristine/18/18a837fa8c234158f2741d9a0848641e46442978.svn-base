using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonResource.Models;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;

namespace ebSuccessWebSite.Models
{
    public class NewsModel
    {
        public int Id { get; set; }
        public string NewsTitle { get; set; }
        public string Abstract { get; set; }
        public string NewsContent { get; set; }
        public DateTime NewsDate { get; set; }
        public bool PublishedStatus { get; set; }
        public HttpPostedFileBase NewsFile { get; set; }
        public int FileId { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
        public int Type { get; set; }
        public NewsModel PreviousNews { get; set; }
        public NewsModel NextNews { get; set; }
    }
}