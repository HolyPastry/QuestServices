using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Bakery
{
    public class SceneSetupQuests : SceneSetupScript
    {
        [SerializeField] private List<QuestData> _quests = new();
        [SerializeField] private List<QuestData> _questsToComplete = new();

        public override IEnumerator Routine()
        {
            yield return Flow.Manager().WaitUntilReady;
            yield return Quests.Manager().WaitUntilReady;

            foreach (var quest in _quests)
                Quests.Manager().StartQuest(quest);

            foreach (var quest in _questsToComplete)
                Quests.Manager().ForceComplete(quest);

        }
    }
}