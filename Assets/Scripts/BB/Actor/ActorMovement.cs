using UnityEngine;

namespace BB.Actor
{
    [RequireComponent(typeof(Actor))]
    public sealed class ActorMovement : MonoBehaviour
    {
        [SerializeField] private ActorMovementProperties properties;
        
        private bool _move;
        private Vector3 _destination;
        private float _maxDistanceDelta;
        
        private void Update()
        {
            if (!_move)
                return;
            
            if (Vector3.Distance(transform.position, _destination) <= _maxDistanceDelta)
            {
                transform.position = _destination;
                _move = false;
                return;
            }

            transform.position = Vector3.MoveTowards(
                current: transform.position,
                target: _destination,
                maxDistanceDelta: _maxDistanceDelta);
        }

        public void MoveTo(Vector3 newPosition)
        {
            _destination = newPosition;
            _maxDistanceDelta = properties.MaxDistanceDelta;
            _move = true;
        }
        
        public ActorMovementProperties Properties => properties;
    }
}