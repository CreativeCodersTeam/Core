using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.Windows.Api
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class WinApiConstants
    {
        public const int LOGON32_PROVIDER_DEFAULT = 0;
        
        public const int LOGON32_PROVIDER_WINNT35 = 1;
        
        public const int LOGON32_PROVIDER_WINNT40 = 2;
        
        public const int LOGON32_PROVIDER_WINNT50 = 3;

        public const int LOGON32_LOGON_INTERACTIVE = 2;

        public const int LOGON32_LOGON_NETWORK = 3;
        
        public const int LOGON32_LOGON_BATCH = 4;
            
        public const int LOGON32_LOGON_SERVICE = 5;
            
        public const int LOGON32_LOGON_UNLOCK = 7;
            
        public const int LOGON32_LOGON_NETWORK_CLEARTEXT = 8;
            
        public const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
    }
}