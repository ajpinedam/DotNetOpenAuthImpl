using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DotNetOpenAuth.Messaging.Bindings;

namespace AuthServerImpl.OAuth
{
    public class CryptoKeyStore
    {
        private static readonly List<CryptoKeyStoreEntry> Keys = new List<CryptoKeyStoreEntry>();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public CryptoKey GetKey(string bucket, string handle)
        {
            return Keys.Where(k => k.Bucket == bucket && k.Handle == handle)
                                    .Select(k => k.Key)
                                    .FirstOrDefault();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<KeyValuePair<string, CryptoKey>> GetKeys(string bucket)
        {
            return Keys.Where(k => k.Bucket == bucket)
                                   .OrderByDescending(k => k.Key.ExpiresUtc)
                                   .Select(k => new KeyValuePair<string, CryptoKey>(k.Handle, k.Key));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveKey(string bucket, string handle)
        {
            Keys.RemoveAll(k => k.Bucket == bucket && k.Handle == handle);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StoreKey(string bucket, string handle, CryptoKey key)
        {
            Keys.Add(
                    new CryptoKeyStoreEntry
                    {
                        Bucket = bucket, 
                        Handle = handle, 
                        Key = key
                    }
                );
        }
    }
}