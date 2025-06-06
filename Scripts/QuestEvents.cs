using System;
using Holypastry.Bakery.Quests;

public static partial class QuestEvents
{
    public static Action<QuestData> OnQuestStarted = delegate { };
    public static Action<QuestData> OnQuestInterrupted = delegate { };
    public static Action<Quest, Condition> OnConditionCompleted = delegate { };
    public static Action<Quest, QuestData.Step> OnStepCompleted = delegate { };
    public static Action<Quest> OnQuestCompleted = delegate { };
}
