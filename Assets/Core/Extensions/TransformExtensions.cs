using UnityEngine;

namespace Core.Extensions
{
    public static class TransformExtensions
    {
        public static bool TryGetComponentInParent<TComponent>(this Transform transform, out TComponent outComponent, int maxLevel = -1)
            where TComponent : Component
        {
            var currentParent = transform;
            var currentLevel = 0;
    
            while (currentParent is not null)
            {
                if (maxLevel >= 0 && currentLevel > maxLevel)
                    break;
        
                var component = currentParent.GetComponent<TComponent>();
                if (component is not null)
                {
                    outComponent = component;
                    return true;
                }
        
                currentParent = currentParent.parent;
                currentLevel++;
            }
    
            outComponent = null;
            return false;
        }
    }
}