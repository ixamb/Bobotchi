using System;
using UnityEngine;

namespace Core.Services.Scheduler
{
    public sealed class ActionScheduler : MonoBehaviour
    {
        private Action _action;
        private float _duration;
        private float _elapsed;
        private bool _active = true;
        private SchedulerEndAction _endAction;

        private Action _onDestroyed;
        
        internal void Initialize(Action action, float durationInSeconds, SchedulerEndAction endAction, Action onDestroyed = null)
        {
            _action = action;
            _duration = durationInSeconds;
            _endAction = endAction;
            _elapsed = 0f;
            _active = true;
            _onDestroyed = onDestroyed;
        }

        private void Update()
        {
            if (!_active) return;

            var dt = Time.deltaTime;
            _elapsed += dt;

            if (_elapsed >= _duration)
            {
                _action?.Invoke();

                switch (_endAction)
                {
                    case SchedulerEndAction.Repeat:
                        _elapsed = 0f;
                        return;
                    case SchedulerEndAction.Pause:
                        _active = false;
                        _elapsed = 0f;
                        return;
                    case SchedulerEndAction.Destroy:
                        Destroy(gameObject);
                        return;
                    default:
                        throw new Exception();
                }
            }
        }
        
        public void ChangeDuration(float newDuration)
        {
            _duration = newDuration;
        }
        
        public float RemainingDuration() => _duration - _elapsed;

        internal void Restart() {
            _elapsed = 0f;
            _active = true;
        }

        public void Pause()
        {
            _active = false;
        }

        public void Resume()
        {
            _active = true;
        }

        public void Stop()
        {
            _active = false;
            _elapsed = 0f;
        }

        private void OnDestroy()
        {
            _onDestroyed?.Invoke();
        }
    }
}