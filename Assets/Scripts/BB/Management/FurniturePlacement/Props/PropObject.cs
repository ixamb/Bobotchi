using System;
using System.Collections.Generic;
using BB.Data;
using BB.Grid.Tiles;
using Core.Extensions;
using UnityEditor;
using UnityEngine;
using Vector2Int = Core.Types.Vector2Int;

namespace BB.Management.FurniturePlacement.Props
{
    public class PropObject : MonoBehaviour
    {
        [field: SerializeField] private string guid = string.Empty;

        [SerializeField] private List<DirectionPlacementObject> directionPlacementObjectReferences;
        
        public Guid Guid
        {
            get => string.IsNullOrEmpty(guid) ? Guid.Empty : new Guid(guid);
            private set => guid =  value.ToString();
        }
        
        public Guid PlacementGuid { get; set; }
        public PropCategory PropCategory { get; set; }

        private PropPlacementDirection _direction;
        
#if UNITY_EDITOR
        
        private void OnValidate()
        {
            if (Guid != Guid.Empty)
                return;
            
            Guid = Guid.NewGuid();
            EditorUtility.SetDirty(this);
            EditorApplication.delayCall += DelayedSaveAssets;
        }

        private void DelayedSaveAssets()
        {
            EditorApplication.delayCall -= DelayedSaveAssets;
            if (this != null)
                AssetDatabase.SaveAssets();
        }
#endif
        
        [ContextMenu("Generate new Guid")]
        private void GenerateNewGuidAndSave()
        {
            Guid = Guid.NewGuid();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public void UpdateSortingOrder(Vector2Int baseSize, Vector2Int globalGridPivot)
        {
            if (!TryGetParentTile(out var tile))
                return;
            
            var pivot = tile.GridPosition;
            var opposite = pivot + new Vector2Int(-baseSize.X, baseSize.Y) + new Vector2Int(1, -1);
            var center = new Vector2(((float)pivot.X + opposite.X) / 2,  ((float)pivot.Y + opposite.Y) / 2);

            var distanceFromBottom = Mathf.Sqrt(Mathf.Pow(globalGridPivot.X - center.x, 2) + Mathf.Pow(globalGridPivot.Y - center.y, 2));
            directionPlacementObjectReferences.ForEach(reference => reference.SetSortingOrder((int)(distanceFromBottom * 100)));
        }

        public void OnObjectSelected()
        {
            directionPlacementObjectReferences.ForEach(reference
                => reference.SetColor(reference.GetColor().SetA(.5f)));
        }

        public void OnObjectDeselected()
        {
            directionPlacementObjectReferences.ForEach(reference
                => reference.SetColor(reference.GetColor().SetA(1f)));
        }

        public void OnObjectValid()
        {
            directionPlacementObjectReferences.ForEach(reference
                => reference.SetColor(reference.GetColor().SetRGBOnly(Color.white)));
        }

        public void OnObjectInvalid()
        {
            directionPlacementObjectReferences.ForEach(reference
                => reference.SetColor(reference.GetColor().SetRGBOnly(Color.red)));
        }

        public PropPlacementDirection Rotation()
        {
            return _direction;
        }

        public void Rotate()
        {
            var newDirection = (PropPlacementDirection)(((int)_direction + 1) % Enum.GetNames(typeof(PropPlacementDirection)).Length);
            Rotate(newDirection);
        }
        
        public void Rotate(PropPlacementDirection direction)
        {
            _direction = direction;
            directionPlacementObjectReferences
                .ForEach(reference => reference.SetDirectionObjectActive(reference.Direction == direction));
        }

        public void EnableColliderInteraction()
        {
            GetComponent<Collider>().enabled = true;
        }

        public void DisableColliderInteraction()
        {
            GetComponent<Collider>().enabled = false;
        }
        
        public bool TryGetParentTile(out Tile outTile)
        {
            if (transform.TryGetComponentInParent<Tile>(out var tile))
            {
                outTile = tile;
                return true;
            }
            outTile = null;
            return false;
        }

        [Serializable]
        internal sealed class DirectionPlacementObject
        {
            [SerializeField] private PropPlacementDirection direction;
            [SerializeField] private SpriteRenderer spriteRenderer;
            
            public PropPlacementDirection Direction => direction;

            public Color GetColor() => spriteRenderer.color;
            public void SetColor(Color color) => spriteRenderer.color = color;
            public void SetSortingOrder(int order) => spriteRenderer.sortingOrder = order;
            public void SetDirectionObjectActive(bool setActive) => spriteRenderer.gameObject.SetActive(setActive);
        }
    }
}