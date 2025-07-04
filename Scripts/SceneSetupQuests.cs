using System.Collections;
using System.Collections.Generic;
using Holypastry.Bakery.Flow;
using UnityEngine;

namespace Holypastry.Bakery.Quests
{
    public class SceneSetupQuests : SceneSetupScript
    {
        [SerializeField] private List<QuestData> _quests = new();
        public override IEnumerator Routine()
        {
            yield return FlowServices.WaitUntilReady();
            yield return QuestServices.WaitUntilReady();

            foreach (var quest in _quests)
            {
                QuestServices.StartQuest(quest);
                yield return null;
            }
        }
    }
}