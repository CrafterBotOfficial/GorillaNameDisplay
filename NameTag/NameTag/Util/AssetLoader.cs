using System.IO;
using System.Reflection;
using UnityEngine;

namespace NameTag.Util
{
    internal static class AssetLoader
    {
        internal static Object GetAsset(string Name)
        {
            return assetBundle.LoadAsset(Name);
        }
        private static AssetBundle _assetBundle;
        private static AssetBundle assetBundle
        {
            get
            {
                if (_assetBundle == null)
                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NameTag.Resources.text"))
                    {
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                        _assetBundle = AssetBundle.LoadFromMemory(bytes);
                    }
                return _assetBundle;
            }
        }
    }
}
