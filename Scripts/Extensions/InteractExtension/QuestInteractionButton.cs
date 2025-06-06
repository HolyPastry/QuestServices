using KBCore.Refs;
using UnityEngine;
using UnityEngine.UI;

namespace Holypastry.Bakery.Quests
{
    [RequireComponent(typeof(Button))]
    public class QuestInteractionButton : ValidatedMonoBehaviour
    {
        [SerializeField, Self] private Button _button;
        [SerializeField] private InteractCondition Condition;

        [SerializeField] private bool _toggleInteraction = false;

        void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }
        void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            if (!_toggleInteraction)
            {
                QuestInteractExtension.InteractRequest(Condition);
            }

            else
            {
                if (QuestInteractExtension.HasInteracted(Condition))
                {
                    QuestInteractExtension.ReverseRequest(Condition);
                }
                else
                {
                    QuestInteractExtension.InteractRequest(Condition);
                }
            }

        }

    }


}