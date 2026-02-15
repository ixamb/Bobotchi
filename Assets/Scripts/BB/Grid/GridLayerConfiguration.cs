using System;
using BB.Grid.Tiles;
using Core.Runtime.Utils;
using UnityEngine;

namespace BB.Grid
{
    [CreateAssetMenu(fileName = "Grid Layer Configuration", menuName = "BB/Grid/Layer Configuration")]
    public sealed class GridLayerConfiguration : ScriptableObject
    {
        [SerializeField] private TextAsset layerCsvAsset;

        public TileState[,] LayerCsvAsset()
        {
            if (layerCsvAsset is null || string.IsNullOrWhiteSpace(layerCsvAsset.text))
                return new TileState[0, 0];

            var dimensions = CsvUtils.GetCsvDimensions(layerCsvAsset.text);
            var separator = CsvUtils.GetCsvSeparator(layerCsvAsset.text);

            var states = new TileState[dimensions.rows, dimensions.columns];
            
            var lines = layerCsvAsset.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < dimensions.rows; i++)
            {
                var columns = lines[i].Split(separator);
                for (var j = 0; j < dimensions.columns; j++)
                {
                    states[i, j] = columns[j] switch
                    {
                        "F" => TileState.Free,
                        "O" => TileState.Occupied,
                        "R" => TileState.OutOfReach,
                        _ => states[i, j]
                    };
                }
            }
            return states;
        }
    }
}