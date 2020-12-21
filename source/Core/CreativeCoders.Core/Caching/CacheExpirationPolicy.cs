using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching
{
    [PublicAPI]
    public class CacheExpirationPolicy : ICacheExpirationPolicy
    {
        public CacheExpirationPolicy(CacheExpirationMode expirationMode)
        {
            ExpirationMode = expirationMode;
        }
        
        public static readonly CacheExpirationPolicy NeverExpire = new(CacheExpirationMode.NeverExpire);

        public static CacheExpirationPolicy AfterAbsoluteDateTime(DateTime absoluteDateTime)
        {
            return new(CacheExpirationMode.AbsoluteDateTime)
            {
                AbsoluteDateTime = absoluteDateTime
            };
        }

        public static CacheExpirationPolicy AfterSlidingTimeSpan(TimeSpan slidingTimeSpan)
        {
            return new(CacheExpirationMode.SlidingTimeSpan)
            {
                SlidingTimeSpan = slidingTimeSpan
            };
        }
        
        public CacheExpirationMode ExpirationMode { get; }

        public DateTime AbsoluteDateTime { get; private set; }

        public TimeSpan SlidingTimeSpan { get; private set; }
    }
}
