using System;
using System.Collections;
using System.Collections.Generic;

using Holypastry.Bakery;
using UnityEngine;

namespace Bakery
{
    public class QuestManager : MonoBehaviour, IQuestManager
    {
        [SerializeField] private string _collectionPath = "Quests";
        [SerializeField] private float _refreshRate = 1f;
        [SerializeField] private bool _verbose = false;

        private DataCollection<QuestData> _collection;

        private List<Quest> _ongoingQuests = new();
        private List<QuestData> _completedQuests = new();
        private List<QuestManagerExtension> _extensions = new();

        private bool _isReady = false;

        public WaitUntil WaitUntilReady => new(() => _isReady);

        void Awake()
        {
            _collection = new DataCollection<QuestData>(_collectionPath);
            GetComponents(_extensions);
        }

        void OnDisable()
        {

            Quests.Manager = Quests.UnregisterManager;
        }

        void OnEnable()
        {
            Quests.Manager = () => this;
        }


        protected IEnumerator Start()
        {
            yield return Flow.Manager().WaitUntilReady;
            _ongoingQuests = new();
            _completedQuests = new();

            if (Persistence.Manager().IsEnabled)
                Load();


            foreach (var extension in _extensions)
                extension.Init();

            StartCoroutine(CheckConditionsRoutine());
            _isReady = true;
        }

        public QuestData.Step GetCurrentStep(QuestData data)
        {
            if (data == null)
            {
                Debug.LogWarning("QuestData is null");
                return null;
            }

            var quest = _ongoingQuests.Find(x => x.Data == data);
            if (quest == null)
            {
                Debug.LogWarning($"Quest {data.name} not found");
                return null;
            }

            return quest.CurrentStep;
        }


        public bool IsQuestCompleted(QuestData data)
        {
            return _completedQuests.Contains(data);
        }

        internal void DebugLog(string message)
        {
            if (_verbose)
                Debug.Log($"[QuestManager] {message}");
        }

        public void ForceConditionCheck(QuestData data)
        {
            var quest = _ongoingQuests.Find(x => x.Data == data);
            if (quest == null)
            {
                Debug.LogWarning($"Quest {data.name} not found");
                return;
            }
            if (quest.CheckConditions())
                Save();

        }

        private IEnumerator CheckConditionsRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_refreshRate);
                CheckConditions();
            }
        }

        private void CheckConditions()
        {
            List<Quest> quests = new(_ongoingQuests);

            bool hasChanges = false;
            foreach (var quest in quests)
                hasChanges |= quest.CheckConditions();
            if (hasChanges)
                Save();
        }

        private void Load()
        {
            var serialQuests = Persistence.Manager().LoadOrCreate<SerialQuests>(SerialQuests.SaveKey);


            foreach (var serialQuest in serialQuests.Quests)
            {
                var data = _collection.GetFromName(serialQuest.Key);
                if (data == null)
                {
                    Debug.LogWarning($"Quest {serialQuest.Key} not found");
                    continue;
                }
                DebugLog($"Loading quest {serialQuest.Key} with {serialQuest.Progress.Count} conditions.");
                DebugLog(serialQuest.ProgressString);
                _ongoingQuests.Add(new Quest(serialQuest, data, _verbose));
            }

            foreach (var questName in serialQuests.CompletedQuests)
            {
                var data = _collection.GetFromName(questName);
                if (data == null)
                {
                    Debug.LogWarning($"Completed quest {questName} not found");
                    continue;
                }
                DebugLog($"Loading completed quest {questName}");
                _completedQuests.Add(data);
            }
        }

        private void DebugLog(object progressString)
        {
            throw new NotImplementedException();
        }

        private void Save()
        {
            Persistence.Manager().Cache(SerialQuests.SaveKey, new SerialQuests(_ongoingQuests, _completedQuests));
            foreach (var extension in _extensions)
                extension.Save();

        }
        public void ForceComplete(QuestData data)
        {
            var quest = _ongoingQuests.Find(x => x.Data == data);
            if (quest == null)
            {
                Debug.LogWarning($"Quest {data.name} not found");
                return;
            }
            DebugLog($"Forcing completion of quest {data.name}");
            ForceComplete(quest);
        }

        public void ForceComplete(Quest quest)
        {
            DebugLog($"Completing quest {quest.Data.name}");
            if (quest == null)
            {
                Debug.LogWarning($"Quest not found");
                return;
            }

            _ongoingQuests.Remove(quest);
            _completedQuests.AddUnique(quest.Data);
            Quests.Events.OnQuestCompleted?.Invoke(quest);
            if (quest.Data.IsRepeatable)
                StartQuest(quest.Data);
            Save();
        }
        public void StartQuestByName(string questName)
        {
            var data = _collection.GetFromName(questName);
            if (data == null)
            {
                Debug.LogWarning($"Quest {questName} not found");
                return;
            }
            StartQuest(data);
        }

        public void InterruptQuest(QuestData data)
        {
            DebugLog($"Interrupting quest {data.name}");
            if (data == null)
            {
                Debug.LogWarning($"Quest {data.name} not found");
                return;
            }

            if (_completedQuests.Contains(data) && !data.IsRepeatable)
                DebugLog($"Quest {data.name} already completed");


            var quest = _ongoingQuests.Find(x => x.Data == data);
            if (quest == null)
            {
                DebugLog($"Quest {data.name} not found");
                return;
            }

            _ongoingQuests.Remove(quest);
            Quests.Events.OnQuestInterrupted?.Invoke(data);
            Save();
        }

        public void StartQuest(QuestData data)
        {
            DebugLog($"Starting quest {data.name}");
            if (data == null)
            {
                Debug.LogWarning($"Quest {data.name} not found");
                return;
            }
            if (_completedQuests.Contains(data) && !data.IsRepeatable)
            {
                DebugLog($"Quest {data.name} already completed");
                return;
            }

            var quest = _ongoingQuests.Find(x => x.Data == data);
            if (quest != null)
            {
                DebugLog($"Quest {data.name} already started");
                return;
            }

            quest = new Quest(data, _verbose);
            _ongoingQuests.Add(quest);
            Quests.Events.OnQuestStarted?.Invoke(data);
            Save();
        }
    }
}