using System.Collections.Generic;
using UnityEngine;

namespace NameDisplay
{
    internal class ObjectPoolManager
    {
        internal static ObjectPoolManager Instance { get; private set; }

        internal List<GameObject> AvailableNameTags;
        internal ObjectPoolManager()
        {
            Instance = this;
            AvailableNameTags = new List<GameObject>();
            LoadPool();
        }

        private async void LoadPool()
        {
            Transform MasterObject = new GameObject("NameTag Object Pool").transform; // So other mod creators dont hate me for cluttering their hierarchy
            GameObject Prefab = await AssetLoader.GetNameTagPrefab();

            const int PoolSize = 15; // Slightly larger then the max number of players in a game to make it more robust. FUCK RIG SPAMMERS
            for (int i = 0; i < PoolSize; i++)
            {
                GameObject nameTagObj = GameObject.Instantiate(Prefab, MasterObject);
                nameTagObj.SetActive(false);
                AvailableNameTags.Add(nameTagObj);
            }
        }

        internal GameObject PullObjectFromPool()
        {
            GameObject nameTagObj = AvailableNameTags[0];
            AvailableNameTags.RemoveAt(0);
            nameTagObj.SetActive(true);
            Main.Instance.manualLogSource.LogInfo("Pulled nametag from pool!");
            return nameTagObj;
        }

        internal void ReturnObjectToPool(GameObject nameTagObj)
        {
            if (nameTagObj.TryGetComponent(out Behaviours.NameTag nameTag))
                GameObject.Destroy(nameTag);
            nameTagObj.SetActive(false);
            AvailableNameTags.Add(nameTagObj);
        }
    }
}
