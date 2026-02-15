using UnityEngine;

namespace Core.Runtime.Mechanics.Input.Swipe
{
    [CreateAssetMenu(fileName = "Swipe Options", menuName = "Mechanics/Input/Swipe Options")]
    public sealed class SwipeOptions : ScriptableObject
    {
        [Tooltip("Minimal distance required from mousedown to mouseup to be considered as a swipe.")]
        [SerializeField] private float minimalSwipeDistance;
        
        [Tooltip("Maximum required from mousedown to mouseup to be considered as a swipe.")]
        [SerializeField] private float maximalSwipeTimeInMilliseconds;
        
        public float MinimalSwipeDistance => minimalSwipeDistance;
        public float MaximalSwipeTimeInMilliseconds => maximalSwipeTimeInMilliseconds;
    }
}