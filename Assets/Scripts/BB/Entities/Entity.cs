using System;
using UnityEditor;
using UnityEngine;

namespace BB.Entities
{
    public abstract class Entity : ScriptableObject
    {
        [field: SerializeField]
        private string guid = string.Empty;

        public Guid Guid
        {
            get => string.IsNullOrEmpty(guid) ? Guid.Empty : new Guid(guid);
            private set => guid = value.ToString();
        }
        
        [SerializeField] private new string name;
        [SerializeField] private string description;
        [SerializeField] private string extendedDescription;
        [SerializeField] private Texture2D sprite;
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (Guid != Guid.Empty)
                return;
            
            Guid = Guid.NewGuid();
            EditorUtility.SetDirty(this);
            EditorApplication.delayCall += DelayedSaveAssets;
        }

        [ContextMenu("Generate new GUID")]
        private void GenerateNewGuidAndSave()
        {
            Guid = Guid.NewGuid();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void DelayedSaveAssets()
        {
            EditorApplication.delayCall -= DelayedSaveAssets;
            if (this != null)
                AssetDatabase.SaveAssets();
        }
        
        #endif
        
        public string Name => name;
        public string Description => description;
        public string ExtendedDescription => extendedDescription;
        public Texture2D Texture => sprite;
        public Sprite Sprite => sprite == null ? null : Sprite.Create(sprite, new Rect(0, 0, sprite.width, sprite.height), new Vector2(0.5f, 0.5f));
    }
}