using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Services.Delayer
{
    public sealed class ActionDelayerService : Singleton<ActionDelayerService, IActionDelayerService>, IActionDelayerService
    {
        private Dictionary<string, ActionDelayer> _actionDelayers = new();
        
        protected override void Init()
        {
        }
        
        public void Delay(float durationInSeconds, Action action, string code = "")
        {
            var actionDelayer = Instantiate(new GameObject($"ActionDelayer_{(code == string.Empty ? Guid.NewGuid() : code)}")
                .AddComponent<ActionDelayer>(), gameObject.transform);
            actionDelayer.Initialize(durationInSeconds, action, () => _actionDelayers.Remove(code));
        }

        public void Cancel(string code)
        {
            if (!_actionDelayers.ContainsKey(code))
            {
                _actionDelayers[code].Cancel();
                _actionDelayers.Remove(code);
            }
        }

        public void CancelAll()
        {
            foreach (var actionDelayer in _actionDelayers.Values)
            {
                actionDelayer.Cancel();
            }
            _actionDelayers.Clear();
        }
    }
}