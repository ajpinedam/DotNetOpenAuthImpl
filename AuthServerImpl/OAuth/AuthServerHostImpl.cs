using System;
using System.Linq;
using System.Security.Cryptography;
using DotNetOpenAuth.Messaging.Bindings;
using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.OAuth2.ChannelElements;
using DotNetOpenAuth.OAuth2.Messages;

namespace AuthServerImpl.OAuth
{
    public class AuthServerHostImpl : IAuthorizationServerHost
    {
        public IClientDescription GetClient(string clientIdentifier)
        {
            switch (clientIdentifier)
            {

                case "RP":
                    var allowedCallback = "https://localhost:44300/demo";
                    return new ClientDescription(
                                 "data!",
                                 new Uri(allowedCallback),
                                 ClientType.Confidential);
            }

            return null;
        }

        public bool IsAuthorizationValid(IAuthorizationDescription authorization)
        {
            if (authorization.ClientIdentifier == "RP"
                  && authorization.Scope.Count == 1
                  && authorization.Scope.First() == "https://localhost:44300/demo"
                  && authorization.User == "Max Muster")
            {
                return true;
            }

            return false;
        }

        public AccessTokenResult CreateAccessToken(
                     IAccessTokenRequest accessTokenRequestMessage)
        {

            var token = new AuthorizationServerAccessToken
            {
                Lifetime = TimeSpan.FromMinutes(10)
            };

            var signCert = AuthHelper.LoadCert(Config.STS_CERT);
            token.AccessTokenSigningKey =
                     (RSACryptoServiceProvider)signCert.PrivateKey;

            var encryptCert = AuthHelper.LoadCert(Config.SERVICE_CERT);
            token.ResourceServerEncryptionKey =
                     (RSACryptoServiceProvider)encryptCert.PublicKey.Key;

            var result = new AccessTokenResult(token);
            return result;
        }

        public ICryptoKeyStore CryptoKeyStore
        {
            get
            {
                return new InMemoryCryptoKeyStore();
            }
        }

        public INonceStore NonceStore
        {
            get { return new DummyNonceStore(); }
        }

        public AutomatedAuthorizationCheckResponse
                 CheckAuthorizeClientCredentialsGrant(
                             IAccessTokenRequest accessRequest)
        {
            throw new NotImplementedException();
        }

        public AutomatedUserAuthorizationCheckResponse CheckAuthorizeResourceOwnerCredentialGrant(
                     string userName, 
                     string password,
                     IAccessTokenRequest accessRequest)
        {
            if (userName != "Max Muster" || password != "test123")
            {
                return new AutomatedUserAuthorizationCheckResponse(
                                            accessRequest: accessRequest,
                                            approved: false,
                                            canonicalUserName: null);
            }

            if (accessRequest.Scope.Count != 1
                  || accessRequest.Scope.First() != "http://localhost:43019/demo")
            {
                return new AutomatedUserAuthorizationCheckResponse(
                                            accessRequest: accessRequest,
                                            approved: false,
                                            canonicalUserName: null);
            }

            return new AutomatedUserAuthorizationCheckResponse(
                                            accessRequest: accessRequest,
                                            approved: true,
                                            canonicalUserName: userName);
        }
    }


}