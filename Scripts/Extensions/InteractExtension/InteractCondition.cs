using UnityEngine;

namespace Bakery.Quests
{
    [CreateAssetMenu(fileName = "InteractCondition", menuName = "Bakery/Quests/Conditions/Interact")]
    public class InteractCondition : Condition
    {
        public override bool Check => QuestInteractExtension.HasInteracted(this);

    }
}