using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Services.Scheduler
{
    internal sealed class ActionSchedulerService : Singleton<ActionSchedulerService, IActionSchedulerService>, IActionSchedulerService
    {
        private readonly Dictionary<string, ActionScheduler> _actionSchedulers = new();

        protected override void Init()
        {
        }

        public ActionScheduler CreateScheduler(string code, Action action, float durationInSeconds, SchedulerEndAction endAction)
        {
            if (_actionSchedulers.ContainsKey(code))
            {
                _actionSchedulers[code].Initialize(action, durationInSeconds, endAction, OnSchedulerDestroyed);
                _actionSchedulers[code].Restart();
                return _actionSchedulers[code];
            }
            
            var actionScheduler = Instantiate(new GameObject($"ActionScheduler_{code}").AddComponent<ActionScheduler>(), gameObject.transform);
            actionScheduler.Initialize(action, durationInSeconds, endAction, OnSchedulerDestroyed);
            _actionSchedulers.Add(code, actionScheduler);
            return actionScheduler;

            void OnSchedulerDestroyed() => _actionSchedulers.Remove(code);
        }

        public ActionScheduler GetScheduler(string code)
        {
            if (_actionSchedulers.TryGetValue(code, out var actionScheduler))
                return actionScheduler;
            
            Debug.LogWarning($"Warning: scheduler with code {code} could not be found.");
            return null;
        }

        public void DestroyScheduler(string code)
        {
            if (_actionSchedulers.TryGetValue(code, out var actionScheduler))
            {
                actionScheduler.Stop();
                Destroy(actionScheduler.gameObject);
                _actionSchedulers.Remove(code);
                return;
            }
            
            Debug.LogWarning($"Warning: scheduler with code {code} could not be found.");
        }

        public void DestroyAllSchedulers()
        {
            foreach (var actionScheduler in _actionSchedulers.Values)
            {
                actionScheduler.Stop();
                Destroy(actionScheduler.gameObject);
            }
            _actionSchedulers.Clear();
        }
    }
}