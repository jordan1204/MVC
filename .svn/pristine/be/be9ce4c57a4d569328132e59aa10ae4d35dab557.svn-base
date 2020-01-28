using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonResource.Models;

namespace ebSuccessWebSite.Models
{
    public class MetaHandler : BaseHandler
    {
        public string GetMeta(int pageId)
        {
            var getMeta = db.Meta.Where(x => x.PageId == pageId).SingleOrDefault();
            var metaContent = "\t<meta name=\"keywords\" content=\"" + getMeta.Keywords + "\" />\r\n";
            metaContent += "\t<meta name=\"description\" content=\"" + getMeta.Description + "\" />\r\n";
            metaContent += "\t<meta name=\"generator\" content=\"" + getMeta.Generator + "\" />\r\n";
            return metaContent;
        }
    }
}