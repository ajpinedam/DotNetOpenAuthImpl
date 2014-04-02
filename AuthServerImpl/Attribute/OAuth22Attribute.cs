using System;
using System.Security.Cryptography;
using System.Web.Http;
using System.Web.Http.Controllers;
using AuthServerImpl.OAuth;
using DotNetOpenAuth.OAuth2;

namespace AuthServerImpl.Attribute
{
    public class OAuth22Attribute : AuthorizeAttribute
    {
        readonly String[] _scopes = new String[0];

       public OAuth22Attribute() { }
       public OAuth22Attribute(String scope) { _scopes = new [] { scope }; }
       public OAuth22Attribute(String[] scopes) { _scopes = scopes; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var signCert = AuthHelper.LoadCert(Config.STS_CERT);
            var encryptCert = AuthHelper.LoadCert(Config.SERVICE_CERT);

            var analyzer = new StandardAccessTokenAnalyzer(
                           (RSACryptoServiceProvider)signCert.PublicKey.Key,
                           (RSACryptoServiceProvider)encryptCert.PrivateKey);

            var resourceServer = new ResourceServer(analyzer);

            var token =
                 resourceServer.GetAccessToken(
                       actionContext.ControllerContext.Request, _scopes);

            base.OnAuthorization(actionContext);

        }
    }
}