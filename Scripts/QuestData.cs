using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bakery
{
    [CreateAssetMenu(fileName = "QuestData", menuName = "Bakery/Quests/QuestData", order = 1)]
    public class QuestData : ScriptableObject
    {
        [Serializable]
        public class Step
        {
            public string StepTitle;
            public bool AnyCondition;

            public List<Condition> Conditions = new();

            public List<Result> Results = new();

        }

        public string QuestTitle;
        public string Description;
        public bool IsHidden;
        public bool IsRepeatable;

        public List<Step> Steps = new();

    }
}