using System.Collections;
using System.Collections.Generic;
using Bakery.Flow;
using UnityEngine;

namespace Bakery.Quests
{
    public class SceneSetupQuests : SceneSetupScript
    {
        [SerializeField] private List<QuestData> _quests = new();
        [SerializeField] private List<QuestData> _questsToComplete = new();

        public override IEnumerator Routine()
        {
            yield return FlowServices.WaitUntilReady();
            yield return QuestServices.WaitUntilReady();

            foreach (var quest in _quests)
                QuestServices.StartQuest(quest);

            foreach (var quest in _questsToComplete)
                QuestServices.ForceComplete(quest);

        }
    }
}