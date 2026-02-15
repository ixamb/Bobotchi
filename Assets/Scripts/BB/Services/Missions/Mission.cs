using System;
using System.Collections.Generic;
using Core.Runtime.Systems.Actions;
using UnityEditor;
using UnityEngine;

namespace BB.Services.Missions
{
    [CreateAssetMenu(fileName = "Mission Entry", menuName = "BB/Missions/Entry")]
    public sealed class Mission : ScriptableObject
    {
        [field: SerializeField]
        private string guid = string.Empty;

        public Guid Guid
        {
            get => string.IsNullOrEmpty(guid) ? Guid.Empty : new Guid(guid);
            private set => guid = value.ToString();
        }

        [SerializeField] private string title;
        [SerializeField] private MissionDuration duration;
        [SerializeField] private List<GameAction> endMissionActions;
        
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
        
        public string Title => title;
        public MissionDuration Duration =>  duration;
        public List<GameAction> EndMissionActions => endMissionActions;
    }

    [Serializable]
    public sealed record MissionDuration
    {
        [SerializeField][Range(0,23)] private int hours;
        [SerializeField] [Range(0, 59)] private int minutes;
        [SerializeField] [Range(0, 59)] private int seconds;
        
        public int Hours => hours;
        public int Minutes => minutes;
        public int Seconds => seconds;

        public override string ToString()
        {
            var infosList = new List<string>();
            if (Hours != 0) infosList.Add($"{Hours} heure{(Hours > 1 ? 's' : string.Empty)}");
            if (Minutes != 0) infosList.Add($"{Minutes} minute{(Minutes > 1 ? 's' : string.Empty)}");
            if (Seconds != 0) infosList.Add($"{Seconds} second{(Seconds > 1 ? 's' : string.Empty)}");
            return string.Join(", ", infosList);
        }
    }
}