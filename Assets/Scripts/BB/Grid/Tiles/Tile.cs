using System.Collections.Generic;
using System.Linq;
using BB.Data;
using BB.Grid.Walls;
using BB.Management.FurniturePlacement.Props;
using UnityEngine;
using Vector2Int = Core.Types.Vector2Int;

namespace BB.Grid.Tiles
{
    public sealed partial class Tile : MonoBehaviour
    {
        private TileState _state;

        public TileState State => GetPropAnchor.GetComponentsInChildren<PropObject>()
            .Any(prop => prop.PropCategory == PropCategory.OnGround) ? TileState.Occupied : _state;

        [Header("Anchor points")]
        [SerializeField] private Transform characterAnchorPoint;
        [SerializeField] private Transform propAnchorPoint;
        
        [Header("Tile Renderer")]
        [SerializeField] private SpriteRenderer inboundsTileRenderer;
        [SerializeField] private SpriteRenderer outboundsTileRenderer;
        
        public Vector2Int GridPosition { get; private set; }
        
        public void Initialize(Vector2Int mathematicalPosition, TileState state)
        {
            UpdateState(state);
            GridPosition = mathematicalPosition;
        }

        private void UpdateState(TileState newState)
        {
            switch (newState)
            {
                case TileState.Free:
                case TileState.Occupied:
                {
                    inboundsTileRenderer.gameObject.SetActive(true);
                    outboundsTileRenderer.gameObject.SetActive(false);
                    break;
                }
                case TileState.OutOfReach:
                {
                    inboundsTileRenderer.gameObject.SetActive(false);
                    outboundsTileRenderer.gameObject.SetActive(true);
                    break;
                }
                default: return;
            }
            
            _state = newState;
        }

        public void UpdateRenderer(Sprite sprite)
        {
            inboundsTileRenderer.sprite = sprite;
        }
        
        public Transform GetCharacterAnchor => characterAnchorPoint;
        public Transform GetPropAnchor => propAnchorPoint;
    }
    
    public sealed partial class Tile
    {
        [Header("Walls")]
        [SerializeField] private Wall leftWall;
        [SerializeField] private Wall rightWall;
        
        public void UpdateWalls(Sprite sprite)
        {
            leftWall.UpdateRenderer(sprite);
            rightWall.UpdateRenderer(sprite);
        }
        
        public void SetLeftWallActive(bool active) => leftWall.gameObject.SetActive(active);
        public void SetRightWallActive(bool active) => rightWall.gameObject.SetActive(active);
        
        public Wall LeftWall => leftWall;
        public Wall RightWall => rightWall;
        
        public IEnumerable<Wall> Walls => new List<Wall>
        {
            leftWall,
            rightWall,
        };
    }
}