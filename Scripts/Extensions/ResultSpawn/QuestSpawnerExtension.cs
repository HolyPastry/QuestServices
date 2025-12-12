using System;
using System.Collections.Generic;
using Holypastry.Bakery;
using KBCore.Refs;
using UnityEngine;

namespace Bakery
{

    [RequireComponent(typeof(QuestManager))]
    public class QuestSpawnerExtension : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private QuestManager _questManager;

        private readonly List<QuestSpawner> _spawners = new();

        internal static Action<QuestSpawner> Register = delegate { };
        internal static Action<QuestSpawner> Unregister = delegate { };
        internal static Action<QuestObject> Spawn = delegate { };
        internal static Action<QuestObject> Unspawn = delegate { };


        void OnDisable()
        {
            Register = delegate { };
            Unregister = delegate { };
        }
        void OnEnable()
        {
            Register = (spawner) => _spawners.AddUnique(spawner);
            Unregister = (spawner) => _spawners.Remove(spawner);
            Spawn = OnSpawnRequest;
            Unspawn = OnUnspawnRequest;
        }

        private void OnUnspawnRequest(QuestObject location)
        {
            if (TryGetSpawner(location, out var spawner))
                spawner.Unspawn();
        }

        private void OnSpawnRequest(QuestObject location)
        {
            if (TryGetSpawner(location, out var spawner))
                spawner.Spawn();
        }

        private bool TryGetSpawner(QuestObject location, out QuestSpawner spawner)
        {
            spawner = _spawners.Find(s => s.Location == location);
            if (spawner == null)
            {
                Debug.LogWarning($"No spawner found for location {location}");
                return false;
            }
            return true;
        }
    }
}