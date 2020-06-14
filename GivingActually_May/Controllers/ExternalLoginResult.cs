using Microsoft.AspNet.Membership.OpenAuth;
using System.Web.Mvc;

namespace GivingActually_May.Controllers
{
    internal class ExternalLoginResult : ActionResult
    {
        public ExternalLoginResult(string provider, string returnUrl)
        {
            Provider = provider;
            ReturnUrl = returnUrl;
        }

        public string Provider { get; private set; }
        public string ReturnUrl { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            OpenAuth.RequestAuthentication(Provider, ReturnUrl);
        }
    }
}