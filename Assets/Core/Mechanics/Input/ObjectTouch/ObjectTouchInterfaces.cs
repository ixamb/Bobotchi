using UnityEngine;

namespace Core.Mechanics.Input.ObjectTouch
{
    public interface IObjectTouchReleasedHandler
    {
        void OnObjectTouchReleased(GameObject touchedObject);
    }
}