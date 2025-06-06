using System;

using Bakery.Saves;
using UnityEngine;

namespace Holypastry.Bakery.Quests
{

    public class QuestInteractExtension : QuestManagerExtension
    {

        public static Action<InteractCondition> InteractRequest = delegate { };
        public static Action<InteractCondition> ReverseRequest = delegate { };
        public static Func<InteractCondition, bool> HasInteracted = condition => false;

        private InteractConditions _conditions;


        void OnEnable()
        {
            InteractRequest = (condition) => _conditions.AddUnique(condition);
            HasInteracted = (condition) => _conditions.Contains(condition);
            ReverseRequest = ReverseInteraction;
        }

        void OnDisable()
        {
            InteractRequest = delegate { };
            HasInteracted = condition => false;
            ReverseRequest = delegate { };
        }


        private void ReverseInteraction(InteractCondition condition)
        {
            if (!condition.CanReverse)
            {
                Debug.LogWarning($"Condition {condition.name} cannot be reversed.");
                return;
            }
            _conditions.Conditions.Remove(condition);

        }


        internal override void Init()
        {
            _conditions = SaveServices.Load<InteractConditions>(InteractConditions.SavePath);
            _conditions ??= new();
        }

        internal override void Save()
        {
            if (_conditions == null) return;

            SaveServices.Save(InteractConditions.SavePath, _conditions);
        }
    }


}