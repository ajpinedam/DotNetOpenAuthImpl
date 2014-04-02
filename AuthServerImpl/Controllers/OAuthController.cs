using System.Web.Mvc;
using AuthServerImpl.Models;
using AuthServerImpl.OAuth;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.OAuth2.Messages;

namespace AuthServerImpl.Controllers
{
    public class OAuthController : Controller
    {
        public ActionResult Auth()
        {
            var authSvr = new AuthorizationServer(new AuthServerHostImpl());
            var request = authSvr.ReadAuthorizationRequest(Request);
            Session["request"] = request;
            return View();
        }

        [HttpPost]
        public ActionResult Auth(LoginModel loginData)
        {
            var authSvrHostImpl = new AuthServerHostImpl();
            var ok = (loginData.UserName == "Max Muster" && loginData.Password == "test123");
            if (ok)
            {
                var request = Session["request"] as EndUserAuthorizationRequest;
                var authSvr = new AuthorizationServer(authSvrHostImpl);
                var approval = authSvr.PrepareApproveAuthorizationRequest(request, loginData.UserName, new[] { "http://localhost/demo" });

                return authSvr
                         .Channel
                         .PrepareResponse(approval)
                         .AsActionResult();
            }

            ViewBag.Message = "Wrong username or password!";
            return View();
        }

        public ActionResult Token()
        {
            var authSvr = new AuthorizationServer(new AuthServerHostImpl());
            var response = authSvr.HandleTokenRequest(Request);
            return response.AsActionResult();
        }
    }
}