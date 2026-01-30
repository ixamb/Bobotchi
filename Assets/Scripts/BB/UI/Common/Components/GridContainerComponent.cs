using System.Collections.Generic;
using UnityEngine;

namespace BB.UI.Common.Components
{
    public sealed class GridContainerComponent : MonoBehaviour
    {
        [SerializeField] private Transform content;
        
        private readonly List<GridEntryComponent> _spawnedGridEntries = new();
        
        public void InstantiateGridComponent(GridEntryComponent gridEntryComponent, GridEntryDto gridEntryDto)
        {
            var spawnedEntry = Instantiate(gridEntryComponent, content);
            spawnedEntry.Initialize(gridEntryDto);
            _spawnedGridEntries.Add(spawnedEntry);
        }

        public void Clear()
        {
            _spawnedGridEntries.ForEach(gridEntry => Destroy(gridEntry.gameObject));
            _spawnedGridEntries.Clear();
        }
    }
}