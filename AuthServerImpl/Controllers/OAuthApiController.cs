using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AuthServerImpl.Models;
using AuthServerImpl.OAuth;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;

namespace AuthServerImpl.Controllers
{
    public class OAuthApiController : ApiController
    {

        private readonly AuthorizationServer _authServer;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="authServerHost"></param>
        public OAuthApiController(IAuthorizationServerHost authServerHost)
        {
            _authServer = new AuthorizationServer(authServerHost);
        }


        public OAuthApiController()
        {
            _authServer = new AuthorizationServer(new AuthServerHostImpl());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Api/Auth/CreateAuth")]
        public HttpResponseMessage Auth(LoginModel loginModel)
        {
            if (false)
            {

            }

            var rqt = new HttpRequestWrapper(HttpContext.Current.Request);

            var request = _authServer.ReadAuthorizationRequest();

            if(null == request)
                throw new Exception("AuthorizarionRequest cannot be null");

            var approval = _authServer.PrepareApproveAuthorizationRequest(request, "Max Muster", new[] { "http://localhost:43019/demo" });

            return _authServer
                            .Channel
                            .PrepareResponse(approval)
                            .AsHttpResponseMessage();


        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/Auth/Token")]
        public HttpResponseMessage Token()
        {
            var response = _authServer.HandleTokenRequest(this.Request);
            return response.AsHttpResponseMessage();
        }


        [HttpGet]
        [Route("Api/Auth/GetToken")]
        public HttpResponseMessage GetToken()
        {
            var token = GetAccessTokenFromOwnAuthorizationServer();

            var msg = token != null ? Request.CreateResponse(HttpStatusCode.OK, token) : Request.CreateResponse(HttpStatusCode.BadRequest, "Could not find Token");

            return msg;
        }

        private static IAuthorizationState GetAccessTokenFromOwnAuthorizationServer()
        {
            var server = new AuthorizationServerDescription
            {
                TokenEndpoint = new Uri("https://localhost:44300/Api/Auth/Token"),
                ProtocolVersion = ProtocolVersion.V20
            };

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var client = new UserAgentClient(server, clientIdentifier: "RP");

            client.ClientCredentialApplicator = ClientCredentialApplicator.PostParameter("data!");

            var token = client.ExchangeUserCredentialForToken("Max Muster", "test123", new[] { "https://localhost:44300/demo" });

            return token;
        }

    }


}
