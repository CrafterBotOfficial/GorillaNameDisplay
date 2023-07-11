using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace NameDisplay
{
    internal class ObjectPoolManager
    {
        internal static ObjectPoolManager Instance;

        private List<GameObject> _pool = new List<GameObject>();

        internal ObjectPoolManager()
        {
            if (Instance is object)
                throw new System.Exception("ObjectPoolManager already exists");
            Instance = this;

            FillPool();
        }

        private async void FillPool()
        {
            // Asset loading
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NameDisplay.Resources.text");
            AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromStreamAsync(stream);
            await Task.Run(() => assetBundleCreateRequest.isDone);

            AssetBundleRequest assetBundleRequest = assetBundleCreateRequest.assetBundle.LoadAssetAsync<GameObject>("TextObj");
            await Task.Run(() => assetBundleRequest.isDone);
            GameObject Prefab = assetBundleRequest.asset as GameObject;

            // Pool filling

            Transform MasterObject = new GameObject("NameTag Pool").transform; // Prevents cluttering the hierarchy
            const int poolSize = 11;
            for (int i = 0; i < poolSize; i++)
            {
                GameObject newNameTag = GameObject.Instantiate(Prefab, MasterObject);
                newNameTag.AddComponent<Behaviours.NameTag>();
                newNameTag.SetActive(false);
                _pool.Add(newNameTag);
            }
        }

        /* Controls */

        /// <summary>
        /// Pulls a unused name tag from the pool
        /// </summary>
        internal Behaviours.NameTag Pull()
        {
            GameObject pulled = _pool.First(x => !x.gameObject.activeSelf);
            pulled.SetActive(true);
            return pulled.GetComponent<Behaviours.NameTag>();
        }

        /// <summary>
        /// Returns a object to the pool.
        /// </summary>
        internal void Push(Behaviours.NameTag nameTag)
        {
            Behaviours.NameTag.NameTags.Remove(nameTag.Rig);
            nameTag.gameObject.SetActive(false);
        }
    }
}
