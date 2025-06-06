
using System;
using System.Collections.Generic;
using Bakery.Saves;

namespace Holypastry.Bakery.Quests
{
    [Serializable]
    public class SerialQuests : SerialData
    {
        [Serializable]
        public record ProgressEntry
        {
            public int StepIndex;
            public string ConditionName;
            public bool Completed;

            public ProgressEntry(int stepIndex, string conditionName, bool completed)
            {
                StepIndex = stepIndex;
                ConditionName = conditionName;
                Completed = completed;
            }
        }

        [Serializable]
        public record SerialQuest
        {
            public string Key;

            public List<ProgressEntry> Progress = new();

            public bool IsCompleted;
            public int CurrentStepIndex;

            public SerialQuest(Quest quest)
            {
                Key = quest.Data.name;
                IsCompleted = quest.IsCompleted;
                CurrentStepIndex = quest.CurrentStepIndex;
                foreach ((int stepIndex, Condition condition, bool completed) in quest.Progress)
                    Progress.Add(new(stepIndex, condition.name, completed));
            }

            public string ProgressString
            {
                get
                {
                    string result = "";
                    foreach (var entry in Progress)
                    {
                        result += $"Step {entry.StepIndex}: {entry.ConditionName} - {(entry.Completed ? "Completed" : "Not Completed")}\n";
                    }
                    return result;
                }
            }
        }

        public const string SaveKey = "Quests";
        //public override string Key() => SaveKey;
        public List<SerialQuest> Quests;

        public List<string> CompletedQuests;

        public SerialQuests()
        {
            Quests = new();
            CompletedQuests = new();
        }
        public SerialQuests(List<Quest> quests, List<QuestData> completedQuests)
        {
            Quests = new();
            foreach (var quest in quests)
            {
                var serialQuest = new SerialQuest(quest);
                Quests.Add(serialQuest);
            }
            CompletedQuests = new List<string>();
            foreach (var completedQuest in completedQuests)
            {
                CompletedQuests.Add(completedQuest.name);
            }

        }


    }
}