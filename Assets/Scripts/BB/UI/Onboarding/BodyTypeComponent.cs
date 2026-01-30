using BB.Data;
using UnityEngine;

namespace BB.UI.Onboarding
{
    public class BodyTypeComponent : MonoBehaviour
    {
        [SerializeField] private BodyType bodyType;
        
        public BodyType BodyType => bodyType;
    }
}