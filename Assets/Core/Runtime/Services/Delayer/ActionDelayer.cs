using System;
using UnityEngine;

namespace Core.Runtime.Services.Delayer
{
    public sealed class ActionDelayer : MonoBehaviour
    {
        private Action _action;
        private Action _onDestroy;
        private float _duration;

        private float _elapsed;
        private bool _run = false;
        
        internal void Initialize(float durationInSeconds, Action action, Action onDestroy)
        {
            _action = action;
            _duration = durationInSeconds;
            _run = true;
        }

        private void Update()
        {
            if(!_run)
                return;
            
            var dt = Time.deltaTime;
            _elapsed += dt;

            if (_elapsed >= _duration)
            {
                _action.Invoke();
                _onDestroy.Invoke();
                Destroy(gameObject);
            }
        }

        public void Cancel()
        {
            _run = false;
            _onDestroy.Invoke();
            Destroy(gameObject);
        }
    }
}