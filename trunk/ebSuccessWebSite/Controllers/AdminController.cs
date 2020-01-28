using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ebSuccessWebSite.Models;
using CommonResource.Models;
using System.IO;
using System.Text;



namespace ebSuccessWebSite.Controllers
{
    public class AdminController : Controller
    {
        private AdminSession LoginSession
        {
            get
            {
                return Session["Admin"] as AdminSession;
            }
            set
            {
                Session["Admin"] = value;
            }
        }

        public ActionResult Login()
        {
            LoginModel model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            using (LoginHandler handler = new LoginHandler())
            {
                var IsLogin = handler.CheckAccount(model);
                if (IsLogin)
                {
                    AdminSession session = new AdminSession();
                    session.IsLogin = true;
                    LoginSession = session;             
                    return RedirectToAction("Admin");
                }
                else
                {
                    LoginSession = null;
                    ModelState.AddModelError("Account", "帳號或密碼錯誤");
                    return View(model);               
                }
            }
        }

        public ActionResult Admin()
        {
            if (LoginSession !=null && LoginSession.IsLogin)
            {
                return View();                         
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        #region 內文維護

        public ActionResult Page(string pageName)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (PageHandler handler = new PageHandler())
                {
                    var model = new AdminModel();                 
                    model = handler.GetAdminTemporaryPageContent(pageName);                
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Page(AdminModel model)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (PageHandler handler = new PageHandler())
                {
                    handler.SavePageContent(model);
                    return View("Page", model);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Banner()
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (BannerHandler handler = new BannerHandler())
                {
                    var getBanners = handler.GetBannerList();
                    return View(getBanners);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        [HttpPost]
        public ActionResult Banner(BannerUploadModel model)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                if (String.IsNullOrEmpty(model.Name))
                    return Json(new { Success = false, Message = "Banner名稱為必填" });

                using (BannerHandler handler = new BannerHandler())
                {
                    if (model != null && model.Picture != null && model.Picture.ContentLength > 0)
                    {
                        handler.UploadBanner(model);
                        return Json(new { Success = true });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "請選擇檔案" });
                    }
                }
            }
            else
            {
                return Json(new { Success = false, Message = "請先登入系統在進行操作" });
            }
        }

        [HttpPost]
        public ActionResult ModifyBanner(BannerUploadModel model)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (BannerHandler handler = new BannerHandler())
                {
                    handler.ModifyBanner(model);
                    return Json(new { Success = true });
                }
            }
            else
            {
                return Json(new { Success = false, Message = "請先登入系統在進行操作" });
            }
        }

        [HttpPost]
        public ActionResult DelBanner(int bannerId)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (BannerHandler handler = new BannerHandler())
                {
                    if (bannerId != 0)
                    {
                        handler.DelBanner(bannerId);
                        return Json(new { Success = true });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "查無此檔案" });
                    }          
                }
            }
            else
            {
                return Json(new { Success = false, Message = "請先登入系統在進行操作" });
            }
        }

        [HttpPost]
        public ActionResult GetBanner(int bannerId)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (BannerHandler handler = new BannerHandler())
                {
                    if (bannerId != 0)
                    {
                        var getBanner = handler.GetBanner(bannerId);
                        string getPartialContent = RenderViewToString(ControllerContext, "BannerEditor", getBanner);
                        return Json(new { Title = getBanner.Name, Body = getPartialContent });
                    }
                    else
                    {
                        return Json(new { Title = "查無此檔案", Body = "查無此檔案" });
                    }
                }
            }
            else
            {
                return Json(new { Title = "不允許的操作", Body = "請先登入系統在進行操作" });
            }
        }

        [HttpPost]
        public ActionResult AddUnitForm()
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {          
                var model = new UnitModel();
                string getPartialContent = RenderViewToString(ControllerContext, "UnitCreator", model);
                return Json(new { Title = "新增單元", Body = getPartialContent });
            }
            else
            {
                return Json(new { Title = "不允許的操作", Body = "請先登入系統在進行操作" });
            }
        }

        [HttpPost]
        public ActionResult AddUnit(UnitModel model)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new PageHandler())
                {            
                    if (String.IsNullOrEmpty(model.UnitName))
                    {
                        return Json(new { Success = false, Message = "請輸入單元名稱" });
                    }

                    if (handler.AddUnut(model))
                    {                        
                        return Json(new { Success = true, Message = model.UnitName + "單元新增成功" });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "單元名稱已存在" });
                    }
                }
            }
            else
            {
                return Json(new { Success = false, Message = "請先登入系統在進行操作" });
            }
        }

        public ActionResult UnitList()
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new PageHandler())
                {
                    var getUnitList = handler.GetUnitList();
                    string getPartialContent = RenderViewToString(ControllerContext, "UnitList", getUnitList);
                    return Content(getPartialContent);
                }
            }
            else
            {
                return Content("請先登入再進行操作");
            }
        }

        public ActionResult EditUnit(int id)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new PageHandler())
                {
                    var getUnitInfo = handler.GetUnitById(id);                    
                    return View(getUnitInfo);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult EditPageContent(int contentId, int unitId)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new PageHandler())
                {
                    var getPageInfo = handler.GetPageByContentId(contentId, unitId);
                    return View(getPageInfo);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditUnit(PageAdminModel model)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new PageHandler())
                {             
                    handler.UnitModify(model);
                    var getUpdatedModel = handler.GetUnitById(model.UnitId);
                    return View("EditUnit", getUpdatedModel);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult PageList(int UnitId)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new PageHandler())
                {
                    var getPageList = handler.GetPageList(UnitId);
                    string getPartialContent = RenderViewToString(ControllerContext, "PageList", getPageList);
                    return Content(getPartialContent);
                }
            }
            else
            {
                return Content("請先登入再進行操作");
            }
        }

        [HttpPost]
        public ActionResult AddPageForm(int UnitId)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                var model = new PageContent_Mapping();
                model.UnitId = UnitId;
                string getPartialContent = RenderViewToString(ControllerContext, "PageCreator", model);
                return Json(new { Title = "新增頁面", Body = getPartialContent });
            }
            else
            {
                return Json(new { Title = "不允許的操作", Body = "請先登入系統在進行操作" });
            }
        }

        [HttpPost]
        public ActionResult AddPage(PageContent_Mapping model)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new PageHandler())
                {
                    if (String.IsNullOrEmpty(model.ContentName))
                    {
                        return Json(new { Success = false, Message = "請輸入頁面名稱" });
                    }

                    if (handler.AddPage(model))
                    {
                        return Json(new { Success = true, Message = model.ContentName + "頁面新增成功" });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "頁面名稱已存在" });
                    }
                }
            }
            else
            {
                return Json(new { Success = false, Message = "請先登入系統在進行操作" });
            }
        }

        [HttpPost]
        public ActionResult EnableUnit(int unitId,int option)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new PageHandler())
                {
                    if (handler.EnableUnit(unitId, option))
                    {
                        return Json(new { Success = true, Message = (option == 1) ? "關閉成功" : "開啟成功" });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "找不到該單元資料"});
                    }                 
                }
            }
            else
            {
                return Json(new { Success = false, Message = "請先登入系統在進行操作" });
            }
        }

        [HttpPost]
        public ActionResult DeleteUnit(int unitId)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new PageHandler())
                {
                    if (handler.DeleteUnit(unitId))
                    {
                        return Json(new { Success = true, Message = "刪除成功" });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "找不到該單元資料" });
                    }
                }
            }
            else
            {
                return Json(new { Success = false, Message = "請先登入系統在進行操作" });
            }
        }

        [HttpPost]
        public ActionResult EnablePage(int pageId, int option)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new PageHandler())
                {
                    if (handler.EnablePage(pageId, option))
                    {
                        return Json(new { Success = true, Message = (option == 1) ? "關閉成功" : "開啟成功" });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "找不到該頁面資料" });
                    }
                }
            }
            else
            {
                return Json(new { Success = false, Message = "請先登入系統在進行操作" });
            }
        }

        [HttpPost]
        public ActionResult DeletePage(int pageId)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new PageHandler())
                {
                    if (handler.DeletePage(pageId))
                    {
                        return Json(new { Success = true, Message = "刪除成功" });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "找不到該頁面資料" });
                    }
                }
            }
            else
            {
                return Json(new { Success = false, Message = "請先登入系統在進行操作" });
            }
        }
    
        public ActionResult PageContentModify(int pageContentId)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using(var handler = new PageHandler())
                {
                    var model = handler.GetPageContentByPageContentId(pageContentId);
                    return View(model);         
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult DeletePageContent(int pageContentId)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new PageHandler())
                {
                    if (handler.DeletePageContent(pageContentId))
                    {
                        return Json(new { Success = true, Message = "刪除成功" });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "找不到該次頁面資料" });
                    }
                }
            }
            else
            {
                return Json(new { Success = false, Message = "請先登入系統在進行操作" });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ModifySubPage(PageContent model)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new PageHandler())
                {
                    if (String.IsNullOrEmpty(model.PageTitle))
                    {
                        return Json(new { Success = false, Message = "請輸入次頁面名稱" });
                    }
                    handler.ModifySubPage(model);
                    return Json(new { Success = true, Message = model.PageTitle + "次頁面內容已儲存" });
                }
            }
            else
            {
                return Json(new { Success = false, Message = "請先登入系統在進行操作" });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddSubPageForm(int contentId,int unitId)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                var model = new PageContent();
                model.UnitId = unitId;
                model.ContentId = contentId;
                string getPartialContent = RenderViewToString(ControllerContext, "SubPageCreator", model);
                return Json(new { Title = "新增次頁面", Body = getPartialContent });
            }
            else
            {
                return Json(new { Title = "不允許的操作", Body = "請先登入系統在進行操作" });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateSubPage(PageContent model)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new PageHandler())
                {
                    if (String.IsNullOrEmpty(model.PageTitle))
                    {
                        return Json(new { Success = false, Message = "請輸入次頁面名稱" });
                    }

                    if (handler.AddSubPage(model))
                    {
                        return Json(new { Success = true, Message = model.PageTitle + "次頁面新增成功" });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "次頁面名稱已存在" });
                    }
                }
            }
            else
            {
                return Json(new { Success = false, Message = "請先登入系統在進行操作" });
            }
        }

        public ActionResult PageContentList(int contentId, int unitId)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new PageHandler())
                {
                    var getPageContentList = handler.GetPageContentList(unitId, contentId);
                    string getPartialContent = RenderViewToString(ControllerContext, "PageContentList", getPageContentList);
                    return Content(getPartialContent);
                }
            }
            else
            {
                return Content("請先登入再進行操作");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditPageContent(PageContent_Mapping model)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new PageHandler())
                {
                    handler.PageModify(model);
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult News()
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (NewsHandler handler = new NewsHandler())
                {
                    var model = new NewsModel();
                    model.PublishedStatus = true;
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult GetNews(int newsId)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new NewsHandler())
                {
                    var getNews = handler.GetNews(newsId, true);
                    return View(getNews);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }       

        public ActionResult NewsList()
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (NewsHandler handler = new NewsHandler())
                {
                    var getNewsList = handler.GetNewsList(null, null, true);
                    return PartialView(getNewsList);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PostNews(NewsModel model)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new NewsHandler())
                {
                    var newsId = handler.PostNews(model);                            
                    return RedirectToAction("News");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewsModify(NewsModel model)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (var handler = new NewsHandler())
                {
                    var newsId = handler.NewsModify(model);
                    var getNews = handler.GetNews(newsId, true);        
                    return View("GetNews", getNews);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult DelNews(int newsId)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (NewsHandler handler = new NewsHandler())
                {
                    if (handler.DelNews(newsId))
                    {
                        return Json(new { Success = true });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "查無此新聞" });
                    }          
                }
            }
            else
            {
                return Json(new { Success = false, Message = "請先登入系統在進行操作" });
            }
        }

        [HttpPost]
        public ActionResult DelFile(int newsId)
        {
            if (LoginSession != null && LoginSession.IsLogin)
            {
                using (NewsHandler handler = new NewsHandler())
                {
                    if (handler.DelNewsFile(newsId))
                    {
                        return Json(new { Success = true });
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "查無此檔案" });
                    }
                }
            }
            else
            {
                return Json(new { Success = false, Message = "請先登入系統在進行操作" });
            }
        }

        #endregion

        public class AdminSession
        {
            public bool IsLogin { get; set;}
            public string Name { get; set; }

            public string Email { get; set; }       

        }

        private string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            var viewData = new ViewDataDictionary(model);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}