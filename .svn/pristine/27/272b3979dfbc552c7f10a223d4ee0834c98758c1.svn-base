using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonResource.Models;
using System.Data.Entity.Validation;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;


namespace ebSuccessWebSite.Models
{
    public class BaseHandler : IDisposable
    {
        protected ebWebsiteEntities db;

        public BaseHandler()
        {
            db = new ebWebsiteEntities();
        }

        #region Dispose Method
        // 加下面這段是為了讓handler可以使用using
        // 好處是用完就會馬上結束連線，不用等garbage collection回收
        private bool _disposed = false;
        ~BaseHandler()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                //release managed resources
                if (db != null) db.Dispose();
            }

            //release unmanaged resources
            _disposed = true;
        }
        #endregion
    }

    public class SessionableControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        public SessionableControllerHandler(RouteData routeData)
            : base(routeData)
        { }
    }

    public class SessionStateRouteHandler : IRouteHandler
    {
        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            return new SessionableControllerHandler(requestContext.RouteData);
        }
    }
}