using Core.Runtime.Mechanics.Input.Drag;
using UnityEngine;

namespace BB.Misc
{
    [RequireComponent(typeof(DragHandler))]
    public sealed class CameraDragger : MonoBehaviour, IDragHandler
    {
        [SerializeField] private new Camera camera;
        [SerializeField][Range(0, 1f)] private float dragCoefficient;
        
        public void OnDrag(Vector3 dragDirection)
        {
            var pos = camera.transform.position;
            pos -= dragDirection * dragCoefficient;
            pos.z = camera.transform.position.z;
            camera.transform.position = pos;
        }
    }
}