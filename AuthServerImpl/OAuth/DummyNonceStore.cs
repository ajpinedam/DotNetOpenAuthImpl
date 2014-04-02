using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.Messaging.Bindings;

namespace AuthServerImpl.OAuth
{
    public class DummyNonceStore : INonceStore
    {
        public bool StoreNonce(string context, string nonce, DateTime timestampUtc)
        {
            return true;
        }
    }
}