using System.IO;
using System.Reflection;
using UnityEngine;

namespace NameTag.Core
{
    internal class AssetLoader
    {
        private static AssetBundle assetBundle;
        internal static UnityEngine.Object GetAsset(string Name)
        {
            if (assetBundle == null)
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NameTag.Resources.text");
                byte[] bytes = new byte[stream.Length];

                stream.Read(bytes, 0, bytes.Length);
                stream.Close();

                assetBundle = AssetBundle.LoadFromMemory(bytes);
            }
            return assetBundle.LoadAsset(Name);
        }
    }
}
