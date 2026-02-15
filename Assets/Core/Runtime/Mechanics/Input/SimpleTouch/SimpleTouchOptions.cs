using UnityEngine;

namespace Core.Runtime.Mechanics.Input.SimpleTouch
{
    [CreateAssetMenu(fileName = "Simple Touch Options", menuName = "Mechanics/Input/Simple Touch Options")]
    public sealed class SimpleTouchOptions : ScriptableObject
    {
        [Tooltip("In milliseconds: designate the maximal amount of time between a click down & a click" +
                 "up so that the input is considered as an input click.")]
        [SerializeField] private float clickMaximalDetectionInMilliseconds;
        
        public float ClickMaximalDetectionInMilliseconds => clickMaximalDetectionInMilliseconds;
    }
}