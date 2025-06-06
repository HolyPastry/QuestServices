using System;
using System.Collections;
using UnityEngine;

namespace Holypastry.Bakery.Quests
{
    public class QuestSpawner : MonoBehaviour
    {
        public Location Location;
        public GameObject Prefab;

        public GameObject Instance;

        public void Spawn()
        {
            if (Prefab != null)
                Instance = Instantiate(Prefab, transform);
        }

        IEnumerator Start()
        {
            yield return FlowServices.WaitUntilReady();
            yield return QuestServices.WaitUntilReady();
            if (Prefab == null && Instance == null)
            {
                Debug.LogWarning("You cannot have Prefab and Instance both null: " + gameObject.name);
                yield break;
            }
            if (Location == null)
            {
                Debug.LogWarning("Location is null: " + gameObject.name);
                yield break;
            }

            QuestSpawnerExtension.Register(this);
        }
        void OnDestroy()
        {
            QuestSpawnerExtension.Unregister(this);
        }

        internal void Unspawn()
        {
            if (Instance != null)
            {
                Destroy(Instance);
                Instance = null;
            }
        }
    }
}