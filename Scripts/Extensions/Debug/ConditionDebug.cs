using UnityEngine;

namespace Holypastry.Bakery.Quests
{
    [CreateAssetMenu(fileName = "ConditionDebug", menuName = "Bakery/Quests/Conditions/Debug", order = 1)]
    public class ConditionDebug : Condition
    {
        public bool Completed = false;
        public override bool Check => Completed;
    }
}