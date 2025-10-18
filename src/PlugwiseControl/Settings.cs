using System;
using System.Collections.Generic;

namespace PlugwiseControl;

internal static class Settings {
    public static readonly TimeSpan UsageCacheDuration = TimeSpan.FromSeconds(10);
    public static readonly TimeSpan CircleInfoCacheDuration = TimeSpan.FromMinutes(15);

    public static List<string> CachedMacAddresses = [];
    public static string SerialPort = string.Empty;
}
