using System;
using System.Collections.Generic;
using UnityEngine;

namespace Holypastry.Bakery.Quests
{
    [CreateAssetMenu(fileName = "QuestData", menuName = "Bakery/Quests/QuestData", order = 1)]
    public class QuestData : ContentTag
    {
        [Serializable]
        public class Step
        {
            public string StepTitle;
            public bool AnyCondition;

            public List<Condition> Conditions = new();

            public List<Result> Results = new();

        }

        public const string CollectionPath = "Quests";
        public string Description;
        public bool IsHidden;
        public bool IsRepeatable;

        public List<Step> Steps = new();

    }
}