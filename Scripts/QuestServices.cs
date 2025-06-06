using System;
using Holypastry.Bakery.Quests;
using UnityEngine;

public static partial class QuestServices
{
    public static Action<QuestData> StartQuest = delegate { };
    public static Action<QuestData> InterruptQuest = delegate { };

    public static Action<QuestData> ForceConditionCheck = delegate { };
    public static Action<string> StartQuestByName = delegate { };
    //  public static Action<string> CompleteConditionByName = delegate { };
    //  public static Action<Condition> CompleteCondition = delegate { };

    internal static Func<WaitUntil> WaitUntilReady = () => new WaitUntil(() => true);

    internal static Func<QuestData, bool> IsQuestCompleted = (questData) => false;
}
