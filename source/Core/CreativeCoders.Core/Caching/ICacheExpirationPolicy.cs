namespace CreativeCoders.Core.Caching {
    public interface ICacheExpirationPolicy
    {
        bool CheckIsExpired();
    }
}