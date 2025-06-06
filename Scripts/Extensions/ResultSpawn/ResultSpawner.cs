using UnityEngine;

namespace Holypastry.Bakery.Quests
{
    [CreateAssetMenu(fileName = "ResultSpawner", menuName = "Bakery/Quests/Results/ResultSpawn", order = 1)]
    public class ResultSpawner : Result
    {
        public enum EnumShiva
        {
            Spawn,
            Unspawn
        }
        public Location Location;

        public EnumShiva Action;


        public override void Execute()
        {
            if (Location == null)
            {
                Debug.LogWarning("Location is null");
                return;
            }

            if (Action == EnumShiva.Unspawn)
                QuestSpawnerExtension.Unspawn(Location);
            if (Action == EnumShiva.Spawn)
                QuestSpawnerExtension.Spawn(Location);
        }
    }
}