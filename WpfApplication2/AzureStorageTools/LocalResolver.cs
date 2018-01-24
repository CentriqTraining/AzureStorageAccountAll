using Microsoft.Azure.KeyVault.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AzureStorageTools
{
    public class LocalResolver : IKeyResolver
    {
        private static LocalResolver _Instance;
        private LocalResolver()
        {

        }
        public static LocalResolver Current
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new LocalResolver();
                }
                return _Instance;
            }
        }
        private Dictionary<string, IKey> _Keys = new Dictionary<string, IKey>();
        public void Add(IKey key)
        {
            if (!_Keys.ContainsKey(key.Kid))
            {
                _Keys.Add(key.Kid, key);
            }
        }
        public async Task<IKey> ResolveKeyAsync(string kid, CancellationToken token)
        {
            IKey result;

            _Keys.TryGetValue(kid, out result);
            return await Task.FromResult(result);
        }
    }
}
