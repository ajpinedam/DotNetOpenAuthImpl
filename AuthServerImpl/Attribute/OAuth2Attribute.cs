using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Helpers;
using AuthServerImpl.OAuth;
using DotNetOpenAuth.OAuth2;

using FilterAttribute = System.Web.Http.Filters.FilterAttribute;
using IAuthorizationFilter = System.Web.Http.Filters.IAuthorizationFilter;

namespace AuthServerImpl.Attribute
{
    public class OAuth2Attribute : FilterAttribute, IAuthorizationFilter
    {
        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {

            try
            {
                AntiForgery.Validate();
            }
            catch
            {
                actionContext.Response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    RequestMessage = actionContext.ControllerContext.Request
                };
                return FromResult(actionContext.Response);
            }
            return continuation();
        }

        readonly String[] _scopes = new String[0];

       public OAuth2Attribute() { }
       public OAuth2Attribute(String scope) { _scopes = new [] { scope }; }
       public OAuth2Attribute(String[] scopes) { _scopes = scopes; }

       public void OnAuthorization(AuthorizationContext filterContext)
       {
               var signCert = AuthHelper.LoadCert(Config.STS_CERT);
               var encryptCert = AuthHelper.LoadCert(Config.SERVICE_CERT);

               var analyzer = new StandardAccessTokenAnalyzer(
                              (RSACryptoServiceProvider)signCert.PublicKey.Key,
                              (RSACryptoServiceProvider)encryptCert.PrivateKey);

               var resourceServer = new ResourceServer(analyzer);

               var token = 
                    resourceServer.GetAccessToken(filterContext.HttpContext.Request, _scopes);
       }

       private Task<HttpResponseMessage> FromResult(HttpResponseMessage result)
       {
           var source = new TaskCompletionSource<HttpResponseMessage>();
           source.SetResult(result);
           return source.Task;
       }

    }
}