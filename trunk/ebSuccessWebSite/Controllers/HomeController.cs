using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ebSuccessWebSite.Models;
using CommonResource.Models;
using Resources;
using System.Text.RegularExpressions;


namespace ebSuccessWebSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (MainMenuHandler handle = new MainMenuHandler())
            {
                var model = new MainMenuModel();
                return View(model);
            }            
        }
        public ActionResult FreeTrial()
        {
            var model = new FreeTrialModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FreeTrial(FreeTrialModel model)
        {

            if (!Regex.IsMatch(model.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                ModelState.AddModelError("Email", "Email格式錯誤");
                return View(model);
            }

            if (!Regex.IsMatch(model.Telephone, @"^[0-9]+$"))
            {
                ModelState.AddModelError("Telephone", "電話格式錯誤");
                return View(model);
            }

            if (!Regex.IsMatch(model.Cellphone, @"^(09)\d{8}$"))
            {
                ModelState.AddModelError("Cellphone", "手機格式錯誤");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                using (TrialFormHandler handler = new TrialFormHandler())
                {
                    CustomerTrial_Form form = new CustomerTrial_Form();
                    form.Name = model.Name;
                    form.Telephone = model.Telephone;
                    form.Cellphone = model.Cellphone;
                    form.Email = model.Email;
                    form.Message = model.Message;
                    form.ProductName = model.ProductName;
                    handler.SaveTrialForm(form);
                }
            }

            return View("ThankYou", model);
        }

        public ActionResult MainMenu()
        {
            return Content(Resources.Resource.ResTest);
        }

        [HttpPost]
        public ActionResult UploadPicture(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            string result = "";
            if (upload != null && upload.ContentLength > 0)
            {
                //儲存圖片至Server
                upload.SaveAs(Server.MapPath("~/Content/UploadFiles/Images/" + upload.FileName));


                var imageUrl = Url.Content("~/Content/UploadFiles/Images/" + upload.FileName);

                var vMessage = string.Empty;

                result = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + imageUrl + "\", \"" + vMessage + "\");</script></body></html>";
            }

            return Content(result);
        }

        public ActionResult Page(int UnitId, int? ContentId, int? PageId)
        {
            using (PageHandler handler = new PageHandler())
            {
                var model = new PageModel();
                model = handler.GetPage(UnitId, ContentId, PageId);
                if (model == null)
                    return HttpNotFound();

                ViewBag.MetaDescription = model.MetaDescription;
                ViewBag.MetaKeywords = model.MetaKeywords;
                ViewBag.MetaTitle = model.MetaTitle;
                return View(model);
            }
        }

        public ActionResult PageBlock(string pageName)
        {
            using (PageHandler handler = new PageHandler())
            {
                var model = new PageModel();
                model = handler.GetTemporaryPageContent(pageName);
                return PartialView(model);
            }
        }

        public ActionResult Banners()
        {
            using (BannerHandler handler = new BannerHandler())
            {            
                var model = handler.GetBannerList();
                return PartialView(model);
            }
        }

        public ActionResult PageMenu()
        {
            if (Request.Params["UnitId"] == null)
                return HttpNotFound();

            var getUnitId = Convert.ToInt32(Request.Params["UnitId"]);
            var getContentId = Request.Params["ContentId"] == null ? 0 : Convert.ToInt32(Request.Params["ContentId"]);
            var getPageId = Request.Params["PageId"] == null ? 0: Convert.ToInt32(Request.Params["PageId"]);           

            using (PageHandler handler = new PageHandler())
            {                
                var model = handler.GetPageSubMenu(getUnitId, getContentId, getPageId);
                return PartialView(model);
            }
        }

        public ActionResult UnitBanner()
        {

            if (Request.Params["UnitId"] == null)
                return HttpNotFound();

            var getUnitId = Convert.ToInt32(Request.Params["UnitId"]);

            using (PageHandler handler = new PageHandler())
            {
                var model = handler.GetUnitBanner(getUnitId);
                return PartialView(model);
            }
        }

        public ActionResult PageDesign()
        {
            return View();
        } 
        
        public ActionResult News(int? pageIndex)
        {
            var getPageIndex = Request.Params["Page"];
            var numberOfRecord = 10;
            if (!pageIndex.HasValue)
                pageIndex = 1;
            using (NewsHandler handler = new NewsHandler())
            {
                var getNewsList = handler.GetNewsList(numberOfRecord, pageIndex, false);
                var model = new NewsListModel();
                model.NumberOfTotals = handler.GetNewsCount();
                model.NumberOfRecord = numberOfRecord;
                model.PageIndex = pageIndex.Value;
                model.NewsList = getNewsList;
                return View(model);
            }
        } 

        public ActionResult NewsDetail(int? newsId)
        {
            if (newsId == null)
                return HttpNotFound();

            using (NewsHandler handler = new NewsHandler())
            {
                var getNews = handler.GetNews(newsId.Value, false);
                if (getNews != null)
                {
                    var neighborNews = handler.GetNeighborNews(getNews.Id);

                    var getPreviousNewsIndex = neighborNews.FindIndex(x => x.Type == 1);
                    if (getPreviousNewsIndex >= 0)
                    {
                        getNews.PreviousNews = neighborNews[getPreviousNewsIndex];
                    }
                    var getNextNewsIndex = neighborNews.FindIndex(x => x.Type == 2);
                    if (getNextNewsIndex >= 0)
                    {
                        getNews.NextNews = neighborNews[getNextNewsIndex];
                    }
                                    
                    return View(getNews);
                }
                else
                {
                    return HttpNotFound();
                }

            }
        }
    }
}