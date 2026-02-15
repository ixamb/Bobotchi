using UnityEngine;

namespace Core.Runtime.Mechanics.Input.SimpleTouch
{
    public sealed class SimpleTouchHandler : InputHandler
    {
        [SerializeField] private SimpleTouchOptions options;
        [SerializeField] private InputType inputType;
        
        private ISimpleTouchReleasedHandler[] _simpleTouchReleasedHandlers;

        private bool _isValidTouch;
        private float _startTime;

        private void Awake()
        {
            _simpleTouchReleasedHandlers = GetComponents<ISimpleTouchReleasedHandler>();
        }
        
        protected override void HandleMouse()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                _startTime = Time.time;
                _isValidTouch = TryHandleSimpleTouchPressed(UnityEngine.Input.mousePosition);
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                if (!_isValidTouch)
                    return;
                TryHandleSimpleTouchReleased(UnityEngine.Input.mousePosition);
                _isValidTouch = false;
            }
        }
        
        protected override void HandleTouch()
        {
            if (UnityEngine.Input.touchCount == 0)
                return;

            var touch = UnityEngine.Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startTime = Time.time;
                    _isValidTouch = TryHandleSimpleTouchPressed(touch.position);
                    break;
                
                case TouchPhase.Ended:
                    if (!_isValidTouch)
                        break;
                    TryHandleSimpleTouchReleased(touch.position);
                    _isValidTouch = false;
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                case TouchPhase.Canceled:
                default:
                    break;
            }
        }

        private void PropagateSimpleTouchMessage()
        {
            foreach (var simpleTouchReleasedHandler in _simpleTouchReleasedHandlers)
                simpleTouchReleasedHandler.OnSimpleTouchReleased();
        }

        private bool TryHandleSimpleTouchPressed(Vector3 pressPosition)
        {
            _startTime = Time.time;
            return inputType == InputType.GameObject ? IsTouchOverObject(pressPosition) : IsTouchOverUI(pressPosition);
        }

        private bool TryHandleSimpleTouchReleased(Vector3 releasePosition)
        {
            if (Time.time * 1000 - _startTime * 1000 <= options.ClickMaximalDetectionInMilliseconds)
            {
                PropagateSimpleTouchMessage();
                return true;
            }

            return false;
        }
    }
}