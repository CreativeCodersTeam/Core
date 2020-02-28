using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching
{
    [PublicAPI]
    public class CacheExpirationPolicy : ICacheExpirationPolicy
    {
        private readonly Func<bool> _isExpired;

        public CacheExpirationPolicy(Func<bool> isExpired)
        {
            _isExpired = isExpired;
        }

        public static ICacheExpirationPolicy NeverExpire { get; } = new CacheExpirationPolicy(() => false);

        public static ICacheExpirationPolicy AfterDateTime(DateTime dateTime)
        {
            return new CacheExpirationPolicy(() => DateTime.Now > dateTime);
        }

        public static ICacheExpirationPolicy AfterTimeSpan(TimeSpan timeSpan)
        {
            return AfterDateTime(DateTime.Now.Add(timeSpan));
        }

        public bool CheckIsExpired()
        {
            return _isExpired();
        }
    }
}