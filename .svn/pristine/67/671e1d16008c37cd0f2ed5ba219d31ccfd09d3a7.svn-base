using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonResource.Models;
using System.Net.Mail;
using System.IO;

namespace ebSuccessWebSite.Models
{
    public class PageHandler : BaseHandler
    {
        public AdminModel GetAdminTemporaryPageContent(string actionName)
        {
            var content = db.TemporaryPageContent.Where(x => x.ActionName == actionName).SingleOrDefault();
            if (content != null)
            {
                AdminModel model = new AdminModel();
                model.Content = content.HtmlContent;
                model.MetaDescription = content.MetaDescription;
                model.MetaKeywords = content.MetaKeywords;
                model.MetaTitle = content.MetaTitle;
                return model;
            }
            else
            {
                return null;
            }
        }

        public PageModel GetTemporaryPageContent(string actionName)
        {
            var content = db.TemporaryPageContent.Where(x => x.ActionName == actionName).SingleOrDefault();
            if (content != null)
            {
                PageModel model = new PageModel();
                model.MetaDescription = content.MetaDescription;
                model.MetaKeywords = content.MetaKeywords;
                model.MetaTitle = content.MetaTitle;
                model.PageContent = content.HtmlContent;
                return model;
            }
            else
            {
                return null;
            }
        }  

        public void SavePageContent(AdminModel model)
        {
            if (model != null)
            {
                var getContent = db.TemporaryPageContent.Where(x => x.ActionName == model.PageName).SingleOrDefault();
                if (getContent != null)
                {
                    getContent.HtmlContent = model.Content;
                    getContent.MetaDescription = model.MetaDescription;
                    getContent.MetaKeywords = model.MetaKeywords;
                    getContent.MetaTitle = model.MetaTitle;
                    db.SaveChanges();
                }                 
            }       
        }

        public bool AddUnut(UnitModel model)
        {
            if (model != null)
            {
                var checkExist = db.PageUnit.Where(x => x.UnitName == model.UnitName).Any();
                if (!checkExist)
                {
                    PageUnit entity = new PageUnit();
                    entity.UnitName = model.UnitName;
                    entity.MetaDescription = model.MetaDescription;
                    entity.MetaKeywords = model.MetaKeywords;
                    entity.MetaTitle = model.MetaTitle;
                    entity.HtmlContent = model.HtmlContent;
                    db.PageUnit.Add(entity);
                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public bool AddPage(PageContent_Mapping model)
        {
            if (model != null)
            {
                var checkExist = db.PageContent_Mapping.Where(x => x.ContentName == model.ContentName && x.UnitId == model.UnitId).Any();
                if (!checkExist)
                {
                    PageContent_Mapping entity = new PageContent_Mapping();
                    entity.ContentName = model.ContentName;
                    entity.UnitId = model.UnitId;
                    entity.Enable = true;
                    entity.MainPage = false; 
                    db.PageContent_Mapping.Add(entity);
                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public List<PageUnit> GetUnitList()
        {
            var getUnitList = db.PageUnit.ToList();         
            return getUnitList;
        }

        public List<PageContent_Mapping> GetPageList(int UnitId)
        {
            var getPageList = db.PageContent_Mapping.Where(x=>x.UnitId == UnitId).ToList();
            return getPageList;
        }

        public PageAdminModel GetUnitById(int id)
        {
            var getBasicInfo = db.PageUnit.Where(x=>x.UnitId == id).SingleOrDefault();
            var model = new PageAdminModel();
            model.UnitId = getBasicInfo.UnitId;
            model.UnitName = getBasicInfo.UnitName;
            model.MetaDescription = getBasicInfo.MetaDescription;
            model.MetaKeywords = getBasicInfo.MetaKeywords;
            model.MetaTitle = getBasicInfo.MetaTitle;
            model.HtmlContent = getBasicInfo.HtmlContent;
            model.Enable = getBasicInfo.Enable;
            
            if (getBasicInfo.BannerId != null)
            {
                var getBanner = db.File.Where(x => x.Id == getBasicInfo.BannerId).SingleOrDefault();
                model.PictureData = getBanner.File1;
                model.MimeType = getBanner.MimeType;        
            }
            else
            {
                model.PictureData = null;
                model.MimeType = "";
            }
      
            return model;
        }

        public bool UnitModify(PageAdminModel unit)
        {
            var getUnit = db.PageUnit.Where(x => x.UnitId == unit.UnitId).SingleOrDefault();
            if (getUnit == null)
                return false;
            else
            {
                if (unit.Picture != null && unit.Picture.ContentLength > 0)
                {
                    if (getUnit.BannerId != null)
                    {
                        var getExistFile = db.File.Where(x => x.Id == getUnit.BannerId).SingleOrDefault();
                        db.File.Remove(getExistFile);
                        db.SaveChanges();
                    }

                    CommonResource.Models.File bannerFile = new CommonResource.Models.File();

                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(unit.Picture.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(unit.Picture.ContentLength);
                    }

                    bannerFile.File1 = fileData;
                    bannerFile.MimeType = MimeMapping.GetMimeMapping(unit.Picture.FileName);
                    bannerFile.FileName = unit.Picture.FileName;
                    db.File.Add(bannerFile);
                    db.SaveChanges();

                    getUnit.BannerId = bannerFile.Id;
                }

                getUnit.UnitName = unit.UnitName;
                getUnit.HtmlContent = unit.HtmlContent;
                getUnit.MetaKeywords = unit.MetaKeywords;
                getUnit.MetaDescription = unit.MetaDescription;
                getUnit.MetaTitle = unit.MetaTitle;
             
                db.SaveChanges();
                return true;
            }
        }

        public bool EnableUnit(int unitId,int option)
        {
            var getUnit = db.PageUnit.Where(x => x.UnitId == unitId).SingleOrDefault();
            if (getUnit == null)
                return false;
            else
            {
                getUnit.Enable = (option == 1) ? false : true;
                db.SaveChanges();
                return true;
            }
        }

        public bool DeleteUnit(int unitId)
        {
            var getUnit = db.PageUnit.Where(x => x.UnitId == unitId).SingleOrDefault();
            if (getUnit == null)
                return false;
            else
            {
                var getPageContentList = db.PageContent.Where(x => x.UnitId == unitId).ToList();  
                foreach(var onePageContent in getPageContentList)
                {
                    db.PageContent.Remove(onePageContent);
                }
                db.SaveChanges();

                var getPageList = db.PageContent_Mapping.Where(x => x.UnitId == unitId).ToList();
                foreach (var onePage in getPageList)
                {
                    db.PageContent_Mapping.Remove(onePage);
                }
                db.SaveChanges();

                var getUnitList = db.PageUnit.Where(x => x.UnitId == unitId).ToList();
                foreach (var oneUnit in getUnitList)
                {
                    db.PageUnit.Remove(oneUnit);
                }
                db.SaveChanges();

                return true;
            }
        }

        public bool EnablePage(int pageId, int option)
        {
            var getPage = db.PageContent_Mapping.Where(x => x.ContentId == pageId).SingleOrDefault();
            if (getPage == null)
                return false;
            else
            {
                getPage.Enable = (option == 1) ? false : true;
                db.SaveChanges();
                return true;
            }
        }

        public bool DeletePage(int pageId)
        {
            var getPage = db.PageContent_Mapping.Where(x => x.ContentId == pageId).SingleOrDefault();
            if (getPage == null)
                return false;
            else
            {
                var getPageContentList = db.PageContent.Where(x => x.ContentId == pageId).ToList();
                foreach (var onePageContent in getPageContentList)
                {
                    db.PageContent.Remove(onePageContent);
                }
                db.SaveChanges();

                var getPageList = db.PageContent_Mapping.Where(x => x.ContentId == pageId).ToList();
                foreach (var onePage in getPageList)
                {
                    db.PageContent_Mapping.Remove(onePage);
                }
                db.SaveChanges();
                return true;
            }
        }

        public List<PageContent> GetPageContentByContentId(int contentId,int unitId)
        {
            var getPageContentList = db.PageContent.Where(x => x.ContentId == contentId && x.UnitId == unitId).ToList();
            return getPageContentList;
        }

        public PageContent_Mapping GetPageByContentId(int contentId, int unitId)
        {
            var getPageInfo = db.PageContent_Mapping.Where(x => x.ContentId == contentId && x.UnitId == unitId).SingleOrDefault();
            return getPageInfo;
        }

        public PageContent GetPageContentByPageContentId(int id)
        {
            return db.PageContent.Where(x => x.Id == id).SingleOrDefault();
        }

        public void ModifySubPage(PageContent model)
        {
            var getPageContent = db.PageContent.Where(x => x.Id == model.Id).SingleOrDefault();
            if (getPageContent != null)
            {
                getPageContent.PageTitle = model.PageTitle;
                getPageContent.HtmlContent = model.HtmlContent;
                getPageContent.Enable = model.Enable;
                getPageContent.MetaDescription = model.MetaDescription;
                getPageContent.MetaTitle = model.MetaTitle;
                getPageContent.MetaKeywords = model.MetaKeywords;
                getPageContent.UpdateTime = DateTime.Now;
                getPageContent.DataOrder = model.DataOrder;
                getPageContent.Enable = model.Enable;
                db.SaveChanges();
            }
        }

        public bool DeletePageContent(int pageContentId)
        {
            var getPageContent = db.PageContent.Where(x => x.Id == pageContentId).SingleOrDefault();
            if (getPageContent != null)
            {
                db.PageContent.Remove(getPageContent);
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public bool AddSubPage(PageContent model)
        {
            var checkExist = db.PageContent.Where(x => x.PageTitle == model.PageTitle && x.ContentId == model.ContentId && x.UnitId == model.UnitId).Any();
            if (!checkExist)
            {
                model.UpdateTime = DateTime.Now;
                db.PageContent.Add(model);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PageModify(PageContent_Mapping model)
        {
            var getPage = db.PageContent_Mapping.Where(x => x.ContentId == model.ContentId && x.UnitId == model.UnitId).SingleOrDefault();
            if (getPage != null)
            {
                getPage.ContentName = model.ContentName;
                getPage.Enable = model.Enable;
                getPage.HtmlContent = model.HtmlContent;
                getPage.MetaDescription = model.MetaDescription;
                getPage.MetaKeywords = model.MetaKeywords;
                getPage.MetaTitle = model.MetaTitle;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public List<PageContent> GetPageContentList(int unitId,int contentId)
        {
            return db.PageContent.Where(x => x.UnitId == unitId && x.ContentId == contentId).ToList();
        }

        public PageModel GetPage(int UnitId,int? ContentId,int? PageId)
        {
            var model = new PageModel();
            var breadcrumbModel = new PageModel.Breadcrumb();

            model.BreadcrumbList = new List<PageModel.Breadcrumb>();
            var getUnitName = db.PageUnit.Where(x => x.UnitId == UnitId).SingleOrDefault();
                      
            breadcrumbModel.UnitId = getUnitName.UnitId;
            breadcrumbModel.Name = getUnitName.UnitName;
            model.BreadcrumbList.Add(breadcrumbModel);

            if (ContentId == null)
            {
                var getUnitContent = db.PageUnit.Where(x => x.UnitId == UnitId && x.Enable == true).SingleOrDefault();
                if (getUnitContent != null)
                {
                    model.MetaDescription = getUnitContent.MetaDescription;
                    model.MetaKeywords = getUnitContent.MetaKeywords;
                    model.MetaTitle = getUnitContent.MetaTitle;
                    model.PageTitle = getUnitContent.UnitName;
                    model.PageContent = getUnitContent.HtmlContent;
                }
                else
                {
                    return null;
                }             
            }
            else if (ContentId != null && PageId == null)
            {
                var getPageContent = db.PageContent_Mapping.Where(x => x.UnitId == UnitId && x.ContentId == ContentId && x.Enable == true).SingleOrDefault();
                if (getPageContent != null)
                {
                    model.MetaDescription = getPageContent.MetaDescription;
                    model.MetaKeywords = getPageContent.MetaKeywords;
                    model.MetaTitle = getPageContent.MetaTitle;
                    model.PageTitle = getPageContent.ContentName;
                    model.PageContent = getPageContent.HtmlContent;

                    breadcrumbModel = new PageModel.Breadcrumb();
                    breadcrumbModel.UnitId = getUnitName.UnitId;
                    breadcrumbModel.ContentId = getPageContent.ContentId;
                    breadcrumbModel.Name = getPageContent.ContentName;
                    model.BreadcrumbList.Add(breadcrumbModel);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                var getPageContent = db.PageContent.Where(x => x.UnitId == UnitId && x.ContentId == ContentId && x.Id == PageId && x.Enable == true).SingleOrDefault();
                if (getPageContent != null)
                {
                    model.MetaDescription = getPageContent.MetaDescription;
                    model.MetaKeywords = getPageContent.MetaKeywords;
                    model.MetaTitle = getPageContent.MetaTitle;
                    model.PageTitle = getPageContent.PageTitle;
                    model.PageContent = getPageContent.HtmlContent;

                    breadcrumbModel = new PageModel.Breadcrumb();
                    breadcrumbModel.UnitId = getUnitName.UnitId;
                    breadcrumbModel.ContentId = getPageContent.ContentId;
                    var getContentName = db.PageContent_Mapping.Where(x => x.ContentId == ContentId).SingleOrDefault();
                    breadcrumbModel.Name = getContentName.ContentName;
                    model.BreadcrumbList.Add(breadcrumbModel);

                    breadcrumbModel = new PageModel.Breadcrumb();
                    breadcrumbModel.UnitId = getPageContent.UnitId;
                    breadcrumbModel.ContentId = getPageContent.ContentId;
                    breadcrumbModel.PageId = getPageContent.Id;
                    breadcrumbModel.Name = getPageContent.PageTitle;
                    model.BreadcrumbList.Add(breadcrumbModel);

                }
                else
                {
                    return null;
                }
            }
            return model;
        }

        public PageMenuModel GetPageSubMenu(int UnitId, int ContentId, int PageId)
        {
            var getUnitMenu = (from x in db.PageUnit
                               where x.UnitId == UnitId && x.Enable == true
                               select new PageMenuModel
                               {
                                   Name = x.UnitName,
                                   UnitId = x.UnitId,
                                   Enable = true
                               }).SingleOrDefault();

            if (getUnitMenu != null)
            {

                var getContent = (from x in db.PageContent_Mapping
                                  where x.UnitId == getUnitMenu.UnitId && x.Enable == true
                                  select new PageMenuModel
                                  {
                                      Name = x.ContentName,
                                      UnitId = x.UnitId,
                                      ConentId = x.ContentId,
                                      Enable = (ContentId != 0 && ContentId == x.ContentId) ? true : false
                                  }).ToList();
                getUnitMenu.SubMenu = getContent;

                foreach (var oneContent in getUnitMenu.SubMenu)
                {
                    var getPage = (from x in db.PageContent
                                   where x.UnitId == oneContent.UnitId && x.ContentId == oneContent.ConentId && x.Enable == true
                                   select new PageMenuModel
                                   {
                                       Name = x.PageTitle,
                                       UnitId = x.UnitId,
                                       ConentId = x.ContentId,
                                       PageId = x.Id,
                                       Enable = (PageId != 0 && PageId == x.Id) ? true : false
                                   }).ToList();

                    oneContent.SubMenu = getPage;
                }
            }

            return getUnitMenu;
        }

        public BannerModel GetUnitBanner(int unitId)
        {
            var getBanner = (from x in db.PageUnit
                             join y in db.File on x.BannerId equals y.Id
                             where x.UnitId == unitId
                             select new BannerModel
                             {
                                 MimeType = y.MimeType,
                                 Picture = y.File1  
                             }).SingleOrDefault();            

            return getBanner;
        }
    }
}