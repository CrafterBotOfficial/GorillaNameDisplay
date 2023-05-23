using BepInEx.Logging;

namespace NameTag.Util
{
    internal static class Extensions
    {
        internal static void Log(this object obj, LogLevel logLevel = LogLevel.Info)
        {
#if DEBUG
            Main.Instance.manualLogSource.Log(logLevel, obj);
#endif
        }
    }
}
