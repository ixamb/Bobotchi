using UnityEngine;

namespace BB.UI.Utils
{
    [RequireComponent(typeof(Animator))]
    public class KeepAnimatorStateOnDisable : MonoBehaviour
    {
        private void Awake() => GetComponent<Animator>().keepAnimatorStateOnDisable = true;
    }
}