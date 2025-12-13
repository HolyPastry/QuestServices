

using System;
using UnityEngine;

namespace Bakery
{

    public abstract class Condition : ScriptableObject
    {
        [SerializeField] private float _delay;
        public abstract bool Check { get; }
        public bool CanReverse = false;
    }
}