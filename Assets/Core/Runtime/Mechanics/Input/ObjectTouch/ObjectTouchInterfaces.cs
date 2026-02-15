using UnityEngine;

namespace Core.Runtime.Mechanics.Input.ObjectTouch
{
    public interface IObjectTouchReleasedHandler
    {
        void OnObjectTouchReleased(GameObject touchedObject);
    }
}