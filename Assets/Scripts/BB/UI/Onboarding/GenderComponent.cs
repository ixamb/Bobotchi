using BB.Data;
using UnityEngine;

namespace BB.UI.Onboarding
{
    public class GenderComponent : MonoBehaviour
    {
        [SerializeField] private Gender gender;
        
        public Gender Gender => gender;
    }
}