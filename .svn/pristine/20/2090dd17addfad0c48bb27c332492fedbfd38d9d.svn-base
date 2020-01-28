using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ebSuccessWebSite.Models;

namespace ebSuccessWebSite.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult PageMeta(int pageId)
        {
            using (MetaHandler handler = new MetaHandler())
            {
                var getPageMeta = handler.GetMeta(pageId);
                return Content(getPageMeta);
            } 
        }

        public ActionResult Menu()
        {
            return View();
        }

        public ActionResult Footer()
        {
            return Content("");
        }

        [HttpPost]
        public ActionResult Culture(string CountryCode)
        {
            HttpCookie MyShopInfoIdCookie = new HttpCookie("MyLang", CountryCode);
            MyShopInfoIdCookie.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Add(MyShopInfoIdCookie);
            return RedirectToAction("Index","Home");
        }

        public FileResult GetFile(int fileId)
        {
            using (FileHandler handler = new FileHandler())
            {
                var getFile = handler.GetFile(fileId);
                if (getFile != null)
                    return new FileContentResult(getFile.FileContent, getFile.MimeType) { FileDownloadName = getFile.FileName };
                else
                    return null;
            }
        }

    }
}