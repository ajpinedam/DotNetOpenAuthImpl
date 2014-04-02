using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.Messaging.Bindings;

namespace AuthServerImpl.OAuth
{
    public class InMemoryCryptoKeyStore : ICryptoKeyStore
    {
        public CryptoKey GetKey(string bucket, string handle)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, CryptoKey>> GetKeys(string bucket)
        {
            throw new NotImplementedException();
        }

        public void StoreKey(string bucket, string handle, CryptoKey key)
        {
            throw new NotImplementedException();
        }

        public void RemoveKey(string bucket, string handle)
        {
            throw new NotImplementedException();
        }
    }
}