

using UnityEngine;

namespace Bakery
{

    public abstract class Condition : ScriptableObject
    {
        public abstract bool Check { get; }
        public bool CanReverse = false;
    }
}