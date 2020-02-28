using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching {
    [PublicAPI]
    public class ManuelCacheExpirationPolicy : ICacheExpirationPolicy
    {
        private bool _isExpired;

        public void Expire()
        {
            _isExpired = true;
        }

        public bool CheckIsExpired() => _isExpired;
    }
}