using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using static GivingActually_May.Models.HelperModels.Helper;

namespace GivingActually_May.Models.HelperModels
{
 
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class Authorization : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!UserSession.HasSession)
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                        { "controller", "Home" },
                        { "action", "Login" }
                    });
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class UserAuthorization : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (UserSession.UserRole != RolesEnum.User)
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                        { "controller", "Home" },
                        { "action", "Login" }
                    });
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class AdminAuthorization : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (UserSession.UserRole != RolesEnum.Admin)
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                        { "controller", "Home" },
                        { "action", "Login" }
                    });
            }
        }
    }
}