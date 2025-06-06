
using System;

namespace Holypastry.Bakery.Quests
{

    public abstract class Condition : ContentTag
    {
        public abstract bool Check { get; }
        public bool CanReverse = false;
    }
}