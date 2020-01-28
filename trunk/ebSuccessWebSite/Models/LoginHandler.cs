using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonResource.Models;

namespace ebSuccessWebSite.Models
{
    public class LoginHandler : BaseHandler
    {
        public bool CheckAccount(LoginModel loginInfo)
        {
            if (db.User.Where(x=>x.Email == loginInfo.Account && x.Password == loginInfo.Password).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}