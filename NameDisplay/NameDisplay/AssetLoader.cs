using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace NameDisplay
{
    internal static class AssetLoader
    {
        internal static async Task<GameObject> GetNameTagPrefab()
        {
            AssetBundleRequest request = AssetBundle.LoadAssetAsync("TextObj");
            new WaitUntil(() => request.isDone);
            return request.asset as GameObject;
        }

        private static AssetBundle _assetBundle;
        private static AssetBundle AssetBundle
        {
            get
            {
                if (_assetBundle == null)
                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NameDisplay.Resources.text"))
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
