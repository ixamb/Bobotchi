using System;
using System.Collections.Generic;
using System.Linq;
using BB.Entities;
using JetBrains.Annotations;
using UnityEngine;

namespace BB.Services.Modules.GameData
{
    [CreateAssetMenu(fileName = "Game Data Container", menuName = "BB/Modules/GameData/Game Data Container")]
    public sealed class GameDataContainer : ScriptableObject
    {
        [Header("Visuals")]
        [SerializeField] private List<SpriteEntry> spriteEntries;
        
        [Header("Entities")]
        [SerializeField] private List<Furniture> furnitures = new();
        [SerializeField] private List<Merchant> merchants = new();
        
        [CanBeNull]
        public Sprite GetSprite(string key) => spriteEntries.FirstOrDefault(entry => entry.Key == key)?.Sprite;
        
        public List<Furniture> Furnitures => furnitures;
        public List<Merchant> Merchants => merchants;
    }

    [Serializable]
    public sealed class SpriteEntry
    {
        [SerializeField] private string key;
        [SerializeField] private Sprite sprite;
        
        public string Key => key;
        public Sprite Sprite => sprite;
    }
}