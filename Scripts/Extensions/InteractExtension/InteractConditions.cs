using System;
using System.Collections.Generic;
using Bakery.Saves;
using UnityEngine;

namespace Holypastry.Bakery.Quests
{
    internal class InteractConditions : SerialData
    {
        public const string CollectionPath = "Quests";
        public const string SavePath = "QuestInteractConditions";

        public DataCollection<InteractCondition> Collection { get; private set; }
        public List<string> ConditionNames;

        [NonSerialized]
        public List<InteractCondition> Conditions;

        public InteractConditions()
        {
            Collection ??= new DataCollection<InteractCondition>(CollectionPath);
            Conditions = new();
            ConditionNames = new();

        }


        public override void Serialize()
        {
            base.Serialize();

            foreach (var condition in Conditions)
                ConditionNames.Add(condition.name);

        }
        public override void Deserialize()
        {
            Collection ??= new DataCollection<InteractCondition>(CollectionPath);

            base.Deserialize();
            Conditions = new();
            foreach (var conditionName in ConditionNames)
            {
                var condition = Collection.GetFromName(conditionName);
                if (condition == null)
                {
                    Debug.LogWarning($"InteractCondition with name {conditionName} not found in collection.");
                    continue;
                }
                Conditions.Add(condition);
            }
        }

        public void AddUnique(InteractCondition condition)
        => Conditions.AddUnique(condition);

        public bool Contains(InteractCondition condition)
        => Conditions.Contains(condition);

    }


}