using BB.Data;
using UnityEngine;

namespace BB.Entities
{
    [CreateAssetMenu(fileName = "Surface", menuName = "BB/Entities/Surface")]
    public class Surface : Furniture
    {
        [SerializeField] private SurfaceType surfaceType;
        [SerializeField] private bool isDefault;

        public SurfaceType SurfaceType => surfaceType;
        public bool IsDefault => isDefault;
    }
}