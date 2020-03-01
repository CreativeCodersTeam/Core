namespace CreativeCoders.Core.Caching {
    public enum CacheExpirationMode
    {
        NeverExpire,
        SlidingTimeSpan,
        AbsoluteDateTime
    }
}