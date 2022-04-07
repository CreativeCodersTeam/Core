using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm;

[PublicAPI]
public class FritzBoxConnection
{
    public Uri Url { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public bool AllowUntrustedCertificates { get; set; }
}
