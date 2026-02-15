using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.Runtime.Services.Audio
{
    [CreateAssetMenu(fileName = "Audio Service Properties", menuName = "Core/Services/Audio Service Properties")]
    public sealed class AudioServiceProperties : ScriptableObject
    {
        [SerializeField] private uint canalsCount;
        [SerializeField] private List<AudioClipEntry> audioClipEntries = new();
        
        public uint CanalsCount => canalsCount;

        [CanBeNull]
        public AudioClipEntry GetAudioClipEntry(string code)
        {
            var clipEntry = audioClipEntries.FirstOrDefault(entry => entry.Code.Equals(code));
            if (clipEntry is null)
                Debug.LogWarning($"Warning: audio clip \"{code}\" doesn't exist.");
            return clipEntry;
        }
    }
    
    [Serializable]
    public sealed class AudioClipEntry
    {
        [SerializeField] private AudioType audioType;
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private string code;
        
        public AudioType AudioType => audioType;
        public AudioClip AudioClip => audioClip;
        public string Code => code;
    }

    public enum AudioType
    {
        Music,
        Sfx,
    }
}