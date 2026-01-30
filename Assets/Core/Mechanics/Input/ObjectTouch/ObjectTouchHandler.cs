using UnityEngine;

namespace Core.Mechanics.Input.ObjectTouch
{
    public sealed class ObjectTouchHandler : InputHandler
    {
        [SerializeField] private ObjectTouchOptions options;
        [SerializeField] private InputType inputType;
        
        private IObjectTouchReleasedHandler[] _objectTouchReleasedHandlers;

        private bool _isValidTouch;
        private float _startTime;
        private GameObject _pressedObject;

        private void Awake()
        {
            _objectTouchReleasedHandlers = GetComponents<IObjectTouchReleasedHandler>();
        }
        
        protected override void HandleMouse()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                _startTime = Time.time;
                _pressedObject = GetObjectAtPosition(UnityEngine.Input.mousePosition);
                _isValidTouch = _pressedObject != null;
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                if (!_isValidTouch)
                    return;
                
                TryHandleObjectTouchReleased(UnityEngine.Input.mousePosition);
                _isValidTouch = false;
                _pressedObject = null;
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
                    _pressedObject = GetObjectAtPosition(touch.position);
                    _isValidTouch = _pressedObject != null;
                    break;
                
                case TouchPhase.Ended:
                    if (!_isValidTouch)
                        break;
                    
                    TryHandleObjectTouchReleased(touch.position);
                    _isValidTouch = false;
                    _pressedObject = null;
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                case TouchPhase.Canceled:
                default:
                    break;
            }
        }

        private void TryHandleObjectTouchReleased(Vector3 releasePosition)
        {
            var elapsedTime = (Time.time - _startTime) * 1000f;
            
            if (elapsedTime > options.ClickMaximalDetectionInMilliseconds)
                return;
            
            var releasedObject = GetObjectAtPosition(releasePosition);
            if (releasedObject == _pressedObject && _pressedObject != null)
            {
                PropagateObjectTouchMessage(_pressedObject);
            }
        }

        private void PropagateObjectTouchMessage(GameObject touchedObject)
        {
            foreach (var handler in _objectTouchReleasedHandlers)
                handler.OnObjectTouchReleased(touchedObject);
        }

        private GameObject GetObjectAtPosition(Vector3 screenPosition)
        {
            return inputType == InputType.UI 
                ? GetTouchedUIObject(screenPosition) 
                : GetTouchedGameObject(screenPosition);
        }
        
        private GameObject GetTouchedGameObject(Vector3 screenPosition)
        {
            var ray = camera.ScreenPointToRay(screenPosition);
            
            return Physics.Raycast(ray, out var hit, Mathf.Infinity, inputLayerMask)
                ? hit.collider.gameObject : null;
        }
        
        private static GameObject GetTouchedUIObject(Vector3 screenPosition)
        {
            var eventSystem = UnityEngine.EventSystems.EventSystem.current;
            if (eventSystem == null)
                return null;
            
            var pointerEventData = new UnityEngine.EventSystems.PointerEventData(eventSystem)
            {
                position = screenPosition
            };
            
            var results = new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();
            eventSystem.RaycastAll(pointerEventData, results);
            
            return results.Count > 0 ? results[0].gameObject : null;
        }
    }
}