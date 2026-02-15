using UnityEngine;

namespace Core.Runtime.Mechanics.Input.Drag
{
    public sealed class DragHandler : InputHandler
    {
        [SerializeField] private InputType inputType;
        
        private IDragPressedHandler[] _dragPressedHandlers;
        private IDragHandler[] _dragHandlers;
        private IDragReleasedHandler[] _dragReleasedHandlers;
        
        private bool _isValidDrag;
        private Vector3 _lastDragPosition;

        private void Awake()
        {
            _dragPressedHandlers = GetComponents<IDragPressedHandler>();
            _dragHandlers = GetComponents<IDragHandler>();
            _dragReleasedHandlers = GetComponents<IDragReleasedHandler>();
        }

        protected override void HandleMouse()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                _isValidDrag = TryHandleDragPressed(UnityEngine.Input.mousePosition);
                _lastDragPosition = UnityEngine.Input.mousePosition;
                return;
            }

            if (UnityEngine.Input.GetMouseButton(0))
            {
                if (!_isValidDrag)
                    return;
                
                if (TryHandleDrag(UnityEngine.Input.mousePosition, _lastDragPosition))
                    _lastDragPosition = UnityEngine.Input.mousePosition;
                
                return;
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                if (!_isValidDrag)
                    return;
                
                _isValidDrag = false;
                TryHandleReleasedDrag(UnityEngine.Input.mousePosition);
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
                {
                    _isValidDrag = TryHandleDragPressed(touch.position);
                     _lastDragPosition = touch.position;
                     break;
                }

                case TouchPhase.Moved:
                {
                    if (!_isValidDrag)
                        break;
                    if (TryHandleDrag(touch.position, _lastDragPosition))
                        _lastDragPosition = touch.position;
                    break;
                }

                case TouchPhase.Ended:
                {
                    if (!_isValidDrag)
                        break;
                    _isValidDrag = false;
                    TryHandleReleasedDrag(touch.position);
                    break;
                }

                case TouchPhase.Stationary:
                case TouchPhase.Canceled:
                default: break;
            }
        }

        private bool TryHandleDragPressed(Vector3 pressPosition)
        {
            if (!(inputType == InputType.GameObject ? IsTouchOverObject(pressPosition) : IsTouchOverUI(pressPosition)))
                return false;
            
            foreach (var dragPressedHandler in _dragPressedHandlers)
                dragPressedHandler.OnDragPressed(pressPosition);
            
            return true;
        }

        private bool TryHandleDrag(Vector3 dragPosition, Vector3 lastDragPosition)
        {
            if (!(inputType == InputType.GameObject ? IsTouchOverObject(dragPosition) : IsTouchOverUI(dragPosition)))
                return false;

            var dragDirection = UnityEngine.Input.mousePosition - lastDragPosition;
                
            if (dragDirection == Vector3.zero)
                return false;
                
            foreach (var dragHandler in _dragHandlers)
                dragHandler.OnDrag(dragDirection);

            return true;
        }

        private void TryHandleReleasedDrag(Vector3 releasePosition)
        {
            foreach (var dragReleasedHandler in _dragReleasedHandlers)
                dragReleasedHandler.OnDragReleased(releasePosition);
        }
    }
}