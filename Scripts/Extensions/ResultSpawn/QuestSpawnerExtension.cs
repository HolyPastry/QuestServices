using System;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;

namespace Holypastry.Bakery.Quests
{

    [RequireComponent(typeof(QuestManager))]
    public class QuestSpawnerExtension : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private QuestManager _questManager;

        private readonly List<QuestSpawner> _spawners = new();

        internal static Action<QuestSpawner> Register = delegate { };
        internal static Action<QuestSpawner> Unregister = delegate { };
        internal static Action<Location> Spawn = delegate { };
        internal static Action<Location> Unspawn = delegate { };


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

        private void OnUnspawnRequest(Location location)
        {
            if (TryGetSpawner(location, out var spawner))
                spawner.Unspawn();
        }

        private void OnSpawnRequest(Location location)
        {
            if (TryGetSpawner(location, out var spawner))
                spawner.Spawn();
        }

        private bool TryGetSpawner(Location location, out QuestSpawner spawner)
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