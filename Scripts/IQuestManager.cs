using UnityEngine;


namespace Bakery
{
    public interface IQuestManager
    {
        WaitUntil WaitUntilReady { get; }
        void StartQuest(QuestData data);
        void InterruptQuest(QuestData data);
        void ForceConditionCheck(QuestData data);
        void StartQuestByName(string questName);
        bool IsQuestCompleted(QuestData data);
        QuestData.Step GetCurrentStep(QuestData data);
        void ForceComplete(QuestData data);
        void ForceComplete(Quest quest);

    }
}