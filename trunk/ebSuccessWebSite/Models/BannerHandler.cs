using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonResource.Models;
using System.Net.Mail;
using System.IO;

namespace ebSuccessWebSite.Models
{
    public class BannerHandler : BaseHandler
    {
        public List<BannerModel> GetBannerList()
        {
            var getBanners = (from x in db.HomeBanner
                              join y in db.File on x.FileId equals y.Id
                              select new BannerModel
                              {
                                  Id = x.Id,
                                  Name = x.Name,
                                  Url = x.Url,
                                  Alt = x.Alt,
                                  Picture = y.File1,
                                  MimeType = y.MimeType
                              }).OrderByDescending(x => x.Id).ToList();      
            return getBanners;
        }

        public BannerModel GetBanner(int id)
        {
            var getBanners = (from x in db.HomeBanner
                              join y in db.File on x.FileId equals y.Id
                              where x.Id == id
                              select new BannerModel
                              {
                                  Id = x.Id,
                                  Name = x.Name,
                                  Url = x.Url,
                                  Alt = x.Alt,
                                  Picture = y.File1,
                                  MimeType = y.MimeType
                              }).SingleOrDefault();
            return getBanners;
        }

        public void UploadBanner(BannerUploadModel model)
        {
            if (model != null && model.Picture != null && model.Picture.ContentLength > 0)
            {
                CommonResource.Models.File bannerFile = new CommonResource.Models.File();

                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(model.Picture.InputStream))
                {
                    fileData = binaryReader.ReadBytes(model.Picture.ContentLength);
                }

                bannerFile.File1 = fileData;
                bannerFile.MimeType = MimeMapping.GetMimeMapping(model.Picture.FileName);
                bannerFile.FileName = model.Picture.FileName;
                db.File.Add(bannerFile);
                db.SaveChanges();

                HomeBanner newBanner = new HomeBanner();
                newBanner.Name = model.Name;
                newBanner.Url = model.Url;
                newBanner.Alt = model.Alt;
                newBanner.FileId = bannerFile.Id;
                db.HomeBanner.Add(newBanner);
                db.SaveChanges();
            }            
        }

        public void ModifyBanner(BannerUploadModel model)
        {
            var getBannerInfo = db.HomeBanner.Where(x => x.Id == model.Id).SingleOrDefault();

            if (getBannerInfo != null)
            {
                if (model != null && model.Picture != null && model.Picture.ContentLength > 0)
                {
                    var getBannerFile = db.File.Where(x => x.Id == getBannerInfo.FileId).SingleOrDefault();
                    if (getBannerFile != null)
                    {
                        CommonResource.Models.File bannerFile = new CommonResource.Models.File();

                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(model.Picture.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(model.Picture.ContentLength);
                        }

                        getBannerFile.File1 = fileData;
                        getBannerFile.MimeType = MimeMapping.GetMimeMapping(model.Picture.FileName);
                        getBannerFile.FileName = model.Picture.FileName;
                        db.SaveChanges();
                    }                   
                }

                getBannerInfo.Name = model.Name;
                getBannerInfo.Url = model.Url;
                getBannerInfo.Alt = model.Alt;
                db.SaveChanges();
            }
        }

        public void DelBanner(int id)
        {
            var getBannerInfo = db.HomeBanner.Where(x => x.Id == id).SingleOrDefault();
            if (getBannerInfo != null)
            {
                var getBannerFile = db.File.Where(x => x.Id == getBannerInfo.FileId).SingleOrDefault();
                if (getBannerFile!=null)
                {
                    db.File.Remove(getBannerFile);
                    db.SaveChanges();
                }

                db.HomeBanner.Remove(getBannerInfo);
                db.SaveChanges();
            }
        }
    }
}