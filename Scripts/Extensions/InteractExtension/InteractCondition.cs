using UnityEngine;

namespace Bakery
{
    [CreateAssetMenu(fileName = "InteractCondition", menuName = "Bakery/Quests/Conditions/Interact")]
    public class InteractCondition : Condition
    {
        public override bool Check => QuestInteractExtension.HasInteracted(this);

    }
}