using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonResource.Models;
using System.Net.Mail;
using System.IO;

namespace ebSuccessWebSite.Models
{
    public class FileHandler : BaseHandler
    {
        public FileModel GetFile(int fileId)
        {
            FileModel file = new FileModel();
            var getFile = db.File.Where(x => x.Id == fileId).SingleOrDefault();
            if (getFile != null)
            {
                file.MimeType = getFile.MimeType;
                file.FileContent = getFile.File1;
                file.FileName = getFile.FileName;
                return file;
            }
            else
            {
                return null;
            }               
        } 
    }
}