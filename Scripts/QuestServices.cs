using System;
using Holypastry.Bakery.Quests;
using UnityEngine;

public static class QuestServices
{
    public static Action<QuestData> StartQuest = delegate { };
    public static Action<QuestData> InterruptQuest = delegate { };

    public static Action<QuestData> ForceConditionCheck = delegate { };
    public static Action<string> StartQuestByName = delegate { };

    public static Func<WaitUntil> WaitUntilReady = () => new WaitUntil(() => true);

    public static Func<QuestData, bool> IsQuestCompleted = (questData) => false;

    internal static Action<QuestData> ForceComplete = delegate { };
}
