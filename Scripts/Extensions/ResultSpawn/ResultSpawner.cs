using UnityEngine;

namespace Bakery
{
    [CreateAssetMenu(fileName = "ResultSpawner", menuName = "Bakery/Quests/Results/ResultSpawn", order = 1)]
    public class ResultSpawner : Result
    {
        public enum EnumShiva
        {
            Spawn,
            Unspawn
        }
        public QuestObject Location;

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