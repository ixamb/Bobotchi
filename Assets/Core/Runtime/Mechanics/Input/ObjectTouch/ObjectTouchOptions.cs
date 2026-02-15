using UnityEngine;

namespace Core.Runtime.Mechanics.Input.ObjectTouch
{
    [CreateAssetMenu(fileName = "Object Touch Options", menuName = "Mechanics/Input/Object Touch Options")]
    public sealed class ObjectTouchOptions : ScriptableObject
    {
        [Tooltip("In milliseconds: designate the maximal amount of time between a click down & a click" +
                 "up so that the input is considered as an input click.")]
        [SerializeField] private float clickMaximalDetectionInMilliseconds;
        
        public float ClickMaximalDetectionInMilliseconds => clickMaximalDetectionInMilliseconds;
    }
}