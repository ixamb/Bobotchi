using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Runtime.Mechanics.Input
{
    public abstract class InputHandler : MonoBehaviour
    {
        [Header("Interaction elements")]
        [SerializeField] protected new Camera camera;
        [SerializeField] protected LayerMask inputLayerMask;

        private void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            HandleMouse();
#else
            HandleTouch();
#endif
        }

        protected abstract void HandleMouse();
        protected abstract void HandleTouch();
        
        protected static Direction EvaluateSwipe(Vector2 startDrag, Vector2 endDrag)
        {
            var horizontalShift = startDrag.x - endDrag.x;
            var verticalShift = startDrag.y - endDrag.y;

            switch (horizontalShift)
            {
                case <0:
                    switch (verticalShift)
                    {
                        case < 0: return Mathf.Abs(horizontalShift) < Mathf.Abs(verticalShift) ? Direction.Down : Direction.Left;
                        case >= 0: return Mathf.Abs(horizontalShift) < Mathf.Abs(verticalShift) ? Direction.Up : Direction.Left;
                    } break;
                case >=0:
                    switch (verticalShift)
                    {
                        case < 0: return Mathf.Abs(horizontalShift) < Mathf.Abs(verticalShift) ? Direction.Down : Direction.Right;
                        case >= 0: return Mathf.Abs(horizontalShift) < Mathf.Abs(verticalShift) ? Direction.Up : Direction.Right;
                    } break;
                default: throw new Exception();
            }
            throw new Exception();
        }
        
        protected bool IsTouchOverObject(Vector2 screenPos)
        {
            var ray = camera.ScreenPointToRay(screenPos);
            return Physics.Raycast(ray, out _, Mathf.Infinity, inputLayerMask);
        }
        
        
        protected bool IsTouchOverUI(Vector2 screenPos)
        {
            if (EventSystem.current == null)
                return false;

            var eventData = new PointerEventData(EventSystem.current) {position = screenPos};
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            if (!results.Any())
                return false;
            var topElement = results[0];
            return ((1 << topElement.gameObject.layer) & inputLayerMask) != 0;
        }
    }
}