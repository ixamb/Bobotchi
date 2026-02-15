using System;
using UnityEngine;

namespace Core.Runtime.Mechanics.Input.CardinalDrag
{
    public sealed class CardinalDragHandler : InputHandler
    {
        [SerializeField] private CardinalDragOptions options;
        
        private ILeftCardinalDragHandler[] _leftDragHandlers;
        private IRightCardinalDragHandler[] _rightDragHandlers;
        private IUpCardinalDragHandler[] _upDragHandlers;
        private IDownCardinalDragHandler[] _downDragHandlers;
        private ICardinalDragPressedHandler[] _dragPressedHandlers;
        private ICardinalDragReleasedHandler[] _dragReleasedHandlers;

        private bool _isValidDrag;
        private Vector2 _lastNoticeableDragPosition;
        
        private void Awake()
        {
            _leftDragHandlers = GetComponents<ILeftCardinalDragHandler>();
            _rightDragHandlers = GetComponents<IRightCardinalDragHandler>();
            _upDragHandlers = GetComponents<IUpCardinalDragHandler>();
            _downDragHandlers = GetComponents<IDownCardinalDragHandler>();
            _dragPressedHandlers = GetComponents<ICardinalDragPressedHandler>();
            _dragReleasedHandlers = GetComponents<ICardinalDragReleasedHandler>();
        }

        protected override void HandleMouse()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                _lastNoticeableDragPosition = UnityEngine.Input.mousePosition;
                _isValidDrag = IsTouchOverObject(_lastNoticeableDragPosition);
                foreach (var handler in _dragPressedHandlers) handler.OnCardinalDragPressed();
            }

            if (UnityEngine.Input.GetMouseButton(0))
            {
                if (!_isValidDrag)
                    return;
                
                var position = UnityEngine.Input.mousePosition;
                var distance = Vector2.Distance(position, _lastNoticeableDragPosition);
                if (distance > options.DistanceAsUnit)
                {
                    var dragDirection = EvaluateSwipe(position, _lastNoticeableDragPosition);
                    PropagateDragDirectionMessage(dragDirection, Mathf.RoundToInt(distance/options.DistanceAsUnit));
                    _lastNoticeableDragPosition = position;
                }
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                if (_isValidDrag)
                    return;
                
                foreach (var handler in _dragReleasedHandlers) handler.OnCardinalDragReleased();
            }
        }

        protected override void HandleTouch()
        {
            
        }

        private void PropagateDragDirectionMessage(Direction direction, int dragUnits)
        {
            switch (direction)
            {
                case Direction.Left: foreach (var handler in _leftDragHandlers) handler?.OnLeftCardinalDrag(dragUnits); break;
                case Direction.Right: foreach (var handler in _rightDragHandlers) handler?.OnRightCardinalDrag(dragUnits); break;
                case Direction.Up: foreach (var handler in _upDragHandlers) handler?.OnUpCardinalDrag(dragUnits); break;
                case Direction.Down: foreach (var handler in _downDragHandlers) handler?.OnDownCardinalDrag(dragUnits); break;
                default:
                    throw new Exception();
            }
        }
    }
}