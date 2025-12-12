using UnityEngine;

namespace Bakery
{

    public class DelayCondition : Condition
    {
        public override bool Check => CheckTimer();
    }
}