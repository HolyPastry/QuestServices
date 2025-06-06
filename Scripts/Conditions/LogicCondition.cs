using System.Collections;
using System.Collections.Generic;
using Holypastry.Bakery.Quests;
using UnityEngine;

[CreateAssetMenu(fileName = "LogicCondition", menuName = "Bakery/Quests/Conditions/Logic")]
public class LogicCondition : Condition
{
    public enum LogicType
    {
        All,
        Any
    }
    [SerializeField] private List<Condition> _conditions;
    [SerializeField] private LogicType _logicType = LogicType.All;

    public override bool Check
    {
        get
        {
            if (_logicType == LogicType.All)
                return CheckAllConditions();
            else
                return CheckAnyCondition();
        }
    }


    private bool CheckAllConditions()
    {
        foreach (var condition in _conditions)
        {
            if (!condition.Check)
            {
                return false;
            }
        }
        return true;
    }
    private bool CheckAnyCondition()
    {
        foreach (var condition in _conditions)
        {
            if (condition.Check)
            {
                return true;
            }
        }
        return false;
    }

}
