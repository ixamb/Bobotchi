using System;
using UnityEngine;

namespace Core.Runtime.Mechanics.Input.Swipe
{
    public sealed class SwipeHandler : InputHandler
    {
        [SerializeField] private SwipeOptions options;
        
        private ILeftSwipeHandler[] _leftSwipeHandlers;
        private IRightSwipeHandler[] _rightSwipeHandlers;
        private IUpSwipeHandler[] _upSwipeHandlers;
        private IDownSwipeHandler[] _downSwipeHandlers;
        private ISwipePressedHandler[] _swipePressedHandlers;

        private bool _isValidSwipe;
        private Vector2 _startPosition;
        private float _startTime;
        
        private void Awake()
        {
            _leftSwipeHandlers = GetComponents<ILeftSwipeHandler>();
            _rightSwipeHandlers = GetComponents<IRightSwipeHandler>();
            _upSwipeHandlers = GetComponents<IUpSwipeHandler>();
            _downSwipeHandlers = GetComponents<IDownSwipeHandler>();
            _swipePressedHandlers = GetComponents<ISwipePressedHandler>();
        }
        
        protected override void HandleMouse()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                _startPosition = UnityEngine.Input.mousePosition;
                _startTime = Time.time * 1000;
                _isValidSwipe = IsTouchOverObject(_startPosition);
                
                if (_isValidSwipe)
                    foreach (var handler in _swipePressedHandlers) handler.OnSwipePressed();
            }
                            
            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                if (_isValidSwipe)
                    return;
                                            
                Vector2 endPosition = UnityEngine.Input.mousePosition;
                var swipeTime = Time.time * 1000 - _startTime * 1000;

                if (Vector2.Distance(endPosition, _startPosition) > options.MinimalSwipeDistance
                    && swipeTime < options.MaximalSwipeTimeInMilliseconds)
                {
                    var swipe = EvaluateSwipe(_startPosition, endPosition);
                    PropagateSwipeDirectionMessage(swipe);
                }
                
                _isValidSwipe = false;
            }
        }

        protected override void HandleTouch()
        {
            if (UnityEngine.Input.touchCount == 0) return;
            var touch = UnityEngine.Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                {
                    _startPosition = touch.position;
                    _startTime = Time.time;
                    _isValidSwipe = IsTouchOverObject(_startPosition);
                    break;
                }

                case TouchPhase.Ended:
                {
                    if (!_isValidSwipe)
                        return;
                    
                    var endPosition = touch.position;
                    var swipeTime = Time.time - _startTime;

                    if (Vector2.Distance(endPosition, _startPosition) > options.MinimalSwipeDistance
                        && swipeTime < options.MaximalSwipeTimeInMilliseconds)
                    {
                        var swipe = EvaluateSwipe(_startPosition, touch.position);
                        PropagateSwipeDirectionMessage(swipe);
                    }
                    _isValidSwipe = false;
                    break;
                }
            }
        }

        private void PropagateSwipeDirectionMessage(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left: foreach (var handler in _leftSwipeHandlers) handler?.OnLeftSwipe(); break;
                case Direction.Right: foreach (var handler in _rightSwipeHandlers) handler?.OnRightSwipe(); break;
                case Direction.Up: foreach (var handler in _upSwipeHandlers) handler?.OnUpSwipe(); break;
                case Direction.Down: foreach (var handler in _downSwipeHandlers) handler?.OnDownSwipe(); break;
                default:
                    throw new Exception();
            }
        }
    }
}