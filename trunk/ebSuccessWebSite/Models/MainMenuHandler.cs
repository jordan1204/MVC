using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonResource.Models;

namespace ebSuccessWebSite.Models
{
    public class MainMenuHandler : BaseHandler
    {
        public List<MainMenu> GetMenuList()
        {
            var getMainMenuList = db.MainMenu.ToList();
            return getMainMenuList;
        }
    }
}