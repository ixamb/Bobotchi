using BB.Entities;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Entity), editorForChildClasses: true)]
    public sealed class EntityEditor : UnityEditor.Editor
    {
        private Entity _entity;

        private void OnEnable()
        {
            _entity = target as Entity;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (_entity.Sprite == null)
                return;
            
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("", GUILayout.Width(120), GUILayout.Height(120));
                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), _entity.Texture, ScaleMode.ScaleToFit);
                GUILayout.FlexibleSpace();
            }
        }
    }
}