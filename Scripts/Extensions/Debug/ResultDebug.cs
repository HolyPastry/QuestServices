using KBCore.Refs;
using UnityEngine;

namespace Holypastry.Bakery.Quests
{
    [CreateAssetMenu(fileName = "ResultDebug", menuName = "Bakery/Quests/Results/Debug", order = 1)]
    public class ResultDebug : Result
    {
        public override void Execute()
        {
            Debug.Log("Quest Debug Result Executed:" + name);
        }
    }
}