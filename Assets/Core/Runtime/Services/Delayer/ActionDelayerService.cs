using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.Runtime.Services.Delayer
{
    public sealed class ActionDelayerService : Singleton<ActionDelayerService, IActionDelayerService>, IActionDelayerService
    {
        private readonly Dictionary<string, ActionDelayer> _actionDelayers = new();
        
        protected override void Init()
        {
        }

        [CanBeNull]
        public ActionDelayer Get(string code)
        {
            return _actionDelayers.GetValueOrDefault(code);
        }
        
        public void Delay(float durationInSeconds, Action action, string code = "")
        {
            var delayerCode = $"ActionDelayer_{(code == string.Empty ? Guid.NewGuid() : code)}";
            var actionDelayer = Instantiate(new GameObject(delayerCode).AddComponent<ActionDelayer>(), gameObject.transform);
            actionDelayer.Initialize(durationInSeconds, action, () => _actionDelayers.Remove(code));
            _actionDelayers.Add(code, actionDelayer);
        }

        public void Cancel(string code)
        {
            if (_actionDelayers.Remove(code, out var delayer))
            {
                delayer?.Cancel();
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