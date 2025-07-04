using System;
using System.Collections;
using System.Collections.Generic;

using Bakery.Saves;
using Holypastry.Bakery.Flow;
using UnityEngine;

namespace Holypastry.Bakery.Quests
{


    public class QuestManager : Service
    {
        [SerializeField] private string _collectionPath = "Quests";
        [SerializeField] private float _refreshRate = 1f;
        [SerializeField] private bool _verbose = false;

        private DataCollection<QuestData> _collection;

        private List<Quest> _ongoingQuests = new();
        private List<QuestData> _completedQuests = new();

        internal static Action<Quest> CompleteQuestRequest = delegate { };

        private List<QuestManagerExtension> _extensions = new();

        void Awake()
        {
            _collection = new DataCollection<QuestData>(_collectionPath);
            GetComponents(_extensions);
        }

        void OnDisable()
        {
            QuestServices.WaitUntilReady = () => new WaitUntil(() => true);
            QuestServices.StartQuest = delegate { };
            QuestServices.InterruptQuest = delegate { };
            QuestServices.StartQuestByName = delegate { };
            CompleteQuestRequest = delegate { };
            QuestServices.ForceConditionCheck = delegate { };
            QuestServices.IsQuestCompleted = (questData) => false;
        }

        void OnEnable()
        {
            QuestServices.WaitUntilReady = () => WaitUntilReady;
            QuestServices.StartQuest = StartQuest;
            QuestServices.InterruptQuest = InterruptQuest;
            QuestServices.StartQuestByName = StartQuestByName;
            CompleteQuestRequest = CompleteQuest;
            QuestServices.ForceConditionCheck = CheckConditions;
            QuestServices.IsQuestCompleted = IsQuestCompleted;

        }



        protected override IEnumerator Start()
        {
            yield return FlowServices.WaitUntilReady();
            _ongoingQuests = new();
            _completedQuests = new();

            Load();

            foreach (var extension in _extensions)
                extension.Init();

            StartCoroutine(CheckConditionsRoutine());
            _isReady = true;
        }

        private bool IsQuestCompleted(QuestData data)
        {
            return _completedQuests.Contains(data);
        }

        internal void DebugLog(string message)
        {
            if (_verbose)
                Debug.Log($"[QuestManager] {message}");
        }

        private void CheckConditions(QuestData data)
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
            var serialQuests = SaveServices.Load<SerialQuests>(SerialQuests.SaveKey);
            if (serialQuests == null)
            {
                DebugLog("No saved quests found, starting fresh.");
                return;
            }


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
            SaveServices.Save(SerialQuests.SaveKey, new SerialQuests(_ongoingQuests, _completedQuests));
            foreach (var extension in _extensions)
                extension.Save();

        }

        private void CompleteQuest(Quest quest)
        {
            DebugLog($"Completing quest {quest.Data.name}");
            if (quest == null)
            {
                Debug.LogWarning($"Quest not found");
                return;
            }

            _ongoingQuests.Remove(quest);
            _completedQuests.AddUnique(quest.Data);
            QuestEvents.OnQuestCompleted?.Invoke(quest);
            if (quest.Data.IsRepeatable)
                StartQuest(quest.Data);
            Save();
        }
        private void StartQuestByName(string questName)
        {
            var data = _collection.GetFromName(questName);
            if (data == null)
            {
                Debug.LogWarning($"Quest {questName} not found");
                return;
            }
            StartQuest(data);
        }

        private void InterruptQuest(QuestData data)
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
            QuestEvents.OnQuestInterrupted?.Invoke(data);
            Save();
        }

        private void StartQuest(QuestData data)
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
            QuestEvents.OnQuestStarted?.Invoke(data);
            Save();
        }
    }
}