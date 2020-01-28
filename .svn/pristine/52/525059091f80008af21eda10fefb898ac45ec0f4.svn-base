using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonResource.Models;
using System.Net.Mail;
using System.IO;

namespace ebSuccessWebSite.Models
{
    public class NewsHandler : BaseHandler
    {
        public int GetNewsCount()
        {
            return db.News.ToList().Count();
        }
        public List<NewsModel> GetNewsList(int? numberOfrecord, int? pageIndex, bool admin = false)
        {
            List<NewsModel> newsList = new List<NewsModel>();
            List<News> getNewsList = new List<News>();

            if (numberOfrecord.HasValue && pageIndex.HasValue)
            {
                int numberOfskip = (pageIndex.Value - 1) * numberOfrecord.Value;
                if (admin)
                {
                    getNewsList = db.News.OrderByDescending(x => x.DateTime).Skip(numberOfskip).Take(numberOfrecord.Value).ToList();
                }
                else
                {
                    getNewsList = db.News.Where(x => x.Published == true).OrderByDescending(x => x.DateTime).Skip(numberOfskip).Take(numberOfrecord.Value).ToList();
                }               
            }
            else
            {
                if (admin)
                {
                    getNewsList = db.News.OrderByDescending(x => x.DateTime).ToList();
                }
                else
                {
                    getNewsList = db.News.Where(x => x.Published == true).OrderByDescending(x => x.DateTime).ToList();
                }
                
            }

            foreach (var oneNews in getNewsList)
            {
                var tmpModel = new NewsModel();
                tmpModel.Id = oneNews.Id;
                tmpModel.NewsTitle = oneNews.Title;
                tmpModel.Abstract = oneNews.Abstract;
                tmpModel.NewsContent = oneNews.NewsContent;
                tmpModel.NewsDate = oneNews.DateTime;
                tmpModel.PublishedStatus = oneNews.Published;

                if (oneNews.FileId.HasValue)
                {
                    tmpModel.FileId = oneNews.FileId.Value;
                }

                newsList.Add(tmpModel);
            }
            return newsList;
        }

        public int PostNews(NewsModel model)
        {
            var newNews = new News();

            newNews.Title = model.NewsTitle;
            newNews.NewsContent = model.NewsContent;
            newNews.DateTime = DateTime.Now;
            newNews.Published = model.PublishedStatus;
            newNews.Abstract = model.Abstract;

            if (model.NewsFile != null && model.NewsFile.ContentLength > 0)
            {
                CommonResource.Models.File newsFile = new CommonResource.Models.File();

                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(model.NewsFile.InputStream))
                {
                    fileData = binaryReader.ReadBytes(model.NewsFile.ContentLength);
                }

                newsFile.File1 = fileData;
                newsFile.MimeType = MimeMapping.GetMimeMapping(model.NewsFile.FileName);
                newsFile.FileName = model.NewsFile.FileName;
                db.File.Add(newsFile);
                db.SaveChanges();
                newNews.FileId = newsFile.Id;
            }
                
            db.News.Add(newNews);
            db.SaveChanges();
            return newNews.Id;
        }

        public int NewsModify(NewsModel model)
        {
            var getNews = db.News.Where(x => x.Id == model.Id).SingleOrDefault();
            if (getNews != null)
            {
                getNews.Title = model.NewsTitle;
                getNews.NewsContent = model.NewsContent;
                getNews.DateTime = DateTime.Now;
                getNews.Published = model.PublishedStatus;
                getNews.Abstract = model.Abstract;

                if (model.NewsFile != null && model.NewsFile.ContentLength > 0)
                {
                    var getOldId = getNews.FileId;

                    CommonResource.Models.File newsFile = new CommonResource.Models.File();

                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(model.NewsFile.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(model.NewsFile.ContentLength);
                    }

                    newsFile.File1 = fileData;
                    newsFile.MimeType = MimeMapping.GetMimeMapping(model.NewsFile.FileName);
                    newsFile.FileName = model.NewsFile.FileName;
                    db.File.Add(newsFile);
                    db.SaveChanges();
                    getNews.FileId = newsFile.Id;

                    var getOldFile = db.File.Where(x => x.Id == getOldId).SingleOrDefault();
                    if (getOldFile != null)
                    {
                        db.File.Remove(getOldFile);
                        db.SaveChanges();
                    }   
                }

                db.SaveChanges();              
            }
            return getNews.Id;
        }

        public bool DelNews(int id)
        {
            var getNews = db.News.Where(x => x.Id == id).SingleOrDefault();

            if (getNews == null)
                return false;
            else
            {
                if (getNews.FileId != null && getNews.FileId != 0)
                {
                    var getFile = db.File.Where(x => x.Id == getNews.FileId).SingleOrDefault();
                    if (getFile != null)
                    {
                        db.File.Remove(getFile);
                        db.SaveChanges();
                    }
                }

                db.News.Remove(getNews);
                db.SaveChanges();
                return true;
            }
        }

        public NewsModel GetNews(int newsId, bool admin = false)
        {
            News getNews = null;

            if (admin)
            {
                getNews = db.News.Where(x => x.Id == newsId).SingleOrDefault();
            }
            else
            {
                getNews = db.News.Where(x => x.Id == newsId && x.Published == true).SingleOrDefault();
            }
           
            if (getNews != null)
            {
                var model = new NewsModel();
                model.Id = getNews.Id;
                model.NewsTitle = getNews.Title;
                model.Abstract = getNews.Abstract;
                model.NewsContent = getNews.NewsContent;
                model.NewsDate = getNews.DateTime;
                if (getNews.FileId.HasValue)
                {
                    var getFile = db.File.Where(x => x.Id == getNews.FileId.Value).SingleOrDefault();
                    model.FileName = getFile.FileName;
                }
                model.FileId = getNews.FileId.HasValue ? getNews.FileId.Value : 0;
                model.PublishedStatus = getNews.Published;
                return model;
            }
            else
            {
                return null;
            }
        }

        public List<NewsModel> GetNeighborNews(int newsId)
        {
            var getNewsList = db.News.Where(x => x.Published == true).OrderByDescending(x => x.DateTime).ToList();
            var getIndex = getNewsList.FindIndex(x => x.Id == newsId);
            List<NewsModel> neighborNews = new List<NewsModel>();

            if (getIndex != 0)
            {
                var news = getNewsList[getIndex - 1];
                var tmpModel = new NewsModel();
                tmpModel.Id = news.Id;
                tmpModel.NewsTitle = news.Title;
                tmpModel.Type = 1;           
                neighborNews.Add(tmpModel);
            }

            if ((getIndex + 1) < getNewsList.Count)
            {
                var news = getNewsList[getIndex + 1];
                var tmpModel = new NewsModel();
                tmpModel.Id = news.Id;
                tmpModel.NewsTitle = news.Title;
                tmpModel.Type = 2;
                neighborNews.Add(tmpModel);
            }

            return neighborNews;
        }

        public bool DelNewsFile(int newsId)
        {
            var getNews = db.News.Where(x => x.Id == newsId).SingleOrDefault();
            if (getNews == null)
                return false;
            else
            {
                var getFile = db.File.Where(x => x.Id == getNews.FileId).SingleOrDefault();
                if (getFile == null)
                    return false;
                else
                {
                    db.File.Remove(getFile);
                    db.SaveChanges();

                    getNews.FileId = null;
                    db.SaveChanges();

                    return true;
                }
            }
        }
    }
}