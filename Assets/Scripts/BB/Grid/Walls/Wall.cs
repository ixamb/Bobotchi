using UnityEngine;

namespace BB.Grid.Walls
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private Transform wallPropAnchorPoint;
        [SerializeField] private SpriteRenderer wallRenderer;
        
        public void UpdateRenderer(Sprite sprite) => wallRenderer.sprite = sprite;
    }
}