using System;
using System.Collections;
using System.Collections.Generic;
using Holypastry.Bakery.Quests;
using UnityEngine;


[CreateAssetMenu(fileName = "LogicCondition", menuName = "Bakery/Quests/Conditions/QuestCompleted")]
public class QuestCompletedCondition : Condition
{
    public enum QuestCompletionType
    {
        All,
        Any
    }
    [SerializeField] private List<QuestData> _quests;
    [SerializeField] private QuestCompletionType _type = QuestCompletionType.All;

    public override bool Check => CheckQuests();

    private bool CheckQuests()
    {
        if (_type == QuestCompletionType.All)
            return CheckAllQuests();
        else return CheckAnyQuest();


    }

    private bool CheckAllQuests()
    {
        foreach (var quest in _quests)
        {
            if (!QuestServices.IsQuestCompleted(quest))
            {
                return false;
            }
        }
        return true;
    }

    private bool CheckAnyQuest()
    {
        foreach (var quest in _quests)
        {
            if (QuestServices.IsQuestCompleted(quest))
            {
                return true;
            }
        }
        return false;
    }
}
