using KBCore.Refs;
using UnityEngine;

namespace Holypastry.Bakery.Quests
{
    [RequireComponent(typeof(QuestManager))]
    public abstract class QuestManagerExtension : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private QuestManager _questManager;

        internal abstract void Save();
        internal abstract void Init();
    }
}