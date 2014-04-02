using DotNetOpenAuth.Messaging.Bindings;

namespace AuthServerImpl.OAuth
{
    public class CryptoKeyStoreEntry
    {

        /// <summary>
        /// 
        /// </summary>
        public string Bucket { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Handle { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public CryptoKey Key { get; set; }

    }
}