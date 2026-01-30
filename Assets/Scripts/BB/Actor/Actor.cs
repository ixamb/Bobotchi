using UnityEngine;

namespace BB.Actor
{
    [RequireComponent(typeof(ActorMovement))]
    public sealed class Actor : MonoBehaviour
    {
        private ActorMovement _actorMovement;

        private void Awake()
        {
            _actorMovement = GetComponent<ActorMovement>();
        }

        public void MoveTo(Vector3 position)
        {
            _actorMovement.MoveTo(position);
        }
        
        public ActorMovementProperties MovementProperties => _actorMovement.Properties;
    }
}