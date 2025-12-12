using UnityEngine;

namespace Bakery
{
    public abstract class Result : ScriptableObject
    {
        public abstract void Execute();
    }
}