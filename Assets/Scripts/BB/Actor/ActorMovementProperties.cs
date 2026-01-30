using UnityEngine;

namespace BB.Actor
{
    [CreateAssetMenu(fileName = "Actor Movement Properties", menuName = "BB/Actor/Movement Properties")]
    public sealed class ActorMovementProperties : ScriptableObject
    {
        [SerializeField] private float maxDistanceDelta;
        [SerializeField] private float randomMoveIntervalInSeconds;
        [SerializeField] private int randomMovePerimeter;
        
        public float MaxDistanceDelta => Mathf.Abs(maxDistanceDelta);
        public float RandomMoveIntervalInSeconds => Mathf.Abs(randomMoveIntervalInSeconds);
        public uint RandomMovePerimeter => (uint)Mathf.Abs(randomMovePerimeter);
    }
}