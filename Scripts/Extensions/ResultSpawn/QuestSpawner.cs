using System;
using System.Collections;
using UnityEngine;

namespace Bakery
{
    public class QuestSpawner : MonoBehaviour
    {
        public QuestObject Location;
        public GameObject Prefab;

        public GameObject Instance;

        public void Spawn()
        {
            if (Prefab != null)
                Instance = Instantiate(Prefab, transform);
        }

        IEnumerator Start()
        {
            yield return Flow.Manager().WaitUntilReady;
            yield return Quests.Manager().WaitUntilReady;
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