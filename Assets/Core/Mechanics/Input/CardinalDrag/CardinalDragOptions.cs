using UnityEngine;

namespace Core.Mechanics.Input.CardinalDrag
{
    [CreateAssetMenu(fileName = "Cardinal Drag Options", menuName = "Mechanics/Input/Cardinal Drag Options")]
    public sealed class CardinalDragOptions : ScriptableObject
    {
        [Tooltip("The ratio per unit considering the swipe.")]
        [SerializeField] private float distanceAsUnit;
        
        public float DistanceAsUnit => distanceAsUnit;
    }
}