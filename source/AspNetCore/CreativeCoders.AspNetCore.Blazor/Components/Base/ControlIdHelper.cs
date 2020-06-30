using System;

namespace CreativeCoders.AspNetCore.Blazor.Components.Base
{
    public static class ControlIdHelper
    {
        public static string NewId()
        {
            return "cc_blazor_" + Guid.NewGuid();
        }
    }
}