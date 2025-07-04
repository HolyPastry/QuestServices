
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Holypastry.Bakery.Quests
{

    public class Quest
    {

        public QuestData Data;

        public List<(int stepIndex, Condition condition, bool completed)> Progress = new();

        public bool IsCompleted => _isCompleted;

        private bool _isCompleted = false;

        private bool _verbose;

        private void DebugLog(string message)
        {
            if (_verbose)
                Debug.Log($"[Quest] {Data.name}: {message}");

        }

        public int CurrentStepIndex => _currentStepIndex;
        private int _currentStepIndex = 0;


        public QuestData.Step CurrentStep
        {
            get
            {
                int stepIndex = CurrentStepIndex;
                if (stepIndex == -1 || stepIndex >= Data.Steps.Count) return null;
                return Data.Steps[stepIndex];
            }
        }

        public List<(Condition condition, bool completed)> ActiveConditions
        {
            get
            {
                var list = new List<(Condition, bool)>();
                foreach (var condition in CurrentStep.Conditions)
                {
                    var isCompleted = Progress.Exists(p => p.stepIndex == CurrentStepIndex && p.condition == condition && p.completed);
                    list.Add((condition, isCompleted));
                }
                return list;
            }
        }
        public Quest(QuestData data, bool verbose)
        {
            Data = data;
            _verbose = verbose;
            for (int stepIndex = 0; stepIndex < Data.Steps.Count; stepIndex++)
            {
                var step = Data.Steps[stepIndex];
                foreach (var condition in step.Conditions)
                {
                    Progress.Add((stepIndex, condition, false));
                }
            }
        }

        public Quest(SerialQuests.SerialQuest serialQuest, QuestData data, bool verbose)
        {
            Data = data;
            _currentStepIndex = serialQuest.CurrentStepIndex;
            _isCompleted = serialQuest.IsCompleted;
            _verbose = verbose;

            for (int stepIndex = 0; stepIndex < Data.Steps.Count; stepIndex++)
            {
                var step = Data.Steps[stepIndex];
                foreach (var condition in step.Conditions)
                {
                    bool completed = serialQuest.Progress.Exists(
                        p => p.StepIndex == stepIndex && p.ConditionName == condition.name && p.Completed);
                    Progress.Add((stepIndex, condition, completed));
                }
            }
        }


        public bool IsStepCompleted(QuestData.Step step)
        {
            bool completed = true;
            int stepIndex = Data.Steps.IndexOf(step);
            if (step.AnyCondition)
            {
                completed = Progress.Exists(p => p.stepIndex == stepIndex && p.completed);
                if (_verbose)
                    DebugLog($"Step {stepIndex} ({step.StepTitle}) any condition completed: {completed}");
                return completed;
            }

            foreach (var condition in step.Conditions)
                completed &= Progress.Exists(p => p.stepIndex == stepIndex && p.condition == condition && p.completed);



            return completed;
        }

        internal void UpdateProgress(Condition condition)
        {
            Progress.RemoveAll(x => x.stepIndex == CurrentStepIndex && x.condition == condition);
            Progress.Add((CurrentStepIndex, condition, condition.Check));
        }



        internal bool CheckConditions()
        {
            var currentStep = CurrentStep;
            bool hasChanges = false;
            foreach (var condition in CurrentStep.Conditions)
            {
                bool isCompleted = Progress.Exists(p => p.stepIndex == CurrentStepIndex && p.condition == condition && p.completed);
                if (isCompleted && !condition.CanReverse) continue;

                if ((!isCompleted && condition.Check) ||
                    (isCompleted && condition.CanReverse && !condition.Check))
                {
                    DebugLog($"Condition {condition.name} for step {CurrentStepIndex} ({currentStep.StepTitle}) " +
                             $"{(condition.Check ? "completed" : "reversed")}");
                    hasChanges |= true;
                    UpdateProgress(condition);
                    QuestEvents.OnConditionCompleted?.Invoke(this, condition);
                }

            }
            if (!hasChanges) return false;

            if (IsStepCompleted(currentStep))
            {
                if (_verbose)
                    DebugLog($"Step ({currentStep.StepTitle}) completed.");

                currentStep.Results.ForEach(r => r.Execute());

                QuestEvents.OnStepCompleted?.Invoke(this, CurrentStep);
                _currentStepIndex++;
            }
            if (_currentStepIndex >= Data.Steps.Count)
            {
                _isCompleted = true;
                DebugLog($"Quest {Data.name} completed.");
            }

            if (IsCompleted)
                QuestManager.CompleteQuestRequest?.Invoke(this);

            return true;

        }
    }
}