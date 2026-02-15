using BB.Data;
using BB.Management.FurniturePlacement.Props;
using UnityEngine;
using Vector2Int = Core.Runtime.Types.Vector2Int;

namespace BB.Entities
{
    [CreateAssetMenu(fileName = "Prop", menuName = "BB/Entities/Prop")]
    public class Prop : Furniture
    {
        [SerializeField] private PropCategory category;
        [SerializeField] private PropObject propPrefab;
        
        [SerializeField][Range(Constants.FurnitureDimension.XMinimum, Constants.FurnitureDimension.XMaximum)]
        private int xSize = (int)Constants.FurnitureDimension.XMinimum;
        
        [SerializeField][Range(Constants.FurnitureDimension.YMinimum, Constants.FurnitureDimension.YMaximum)]
        private int ySize = (int)Constants.FurnitureDimension.YMinimum;
        
        public PropCategory Category => category;
        public PropObject PropPrefab => propPrefab;
        
        public Vector2Int  Size => new Vector2Int(xSize, ySize);
    }
}