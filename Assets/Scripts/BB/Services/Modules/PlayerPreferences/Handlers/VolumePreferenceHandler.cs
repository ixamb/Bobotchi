using System;
using BB.Data;
using UnityEngine;
using AudioType = TheForge.Services.Audio.AudioType;

namespace BB.Services.Modules.PlayerPreferences.Handlers
{
    public sealed class VolumePreferenceHandler : PreferenceHandler
    {
        public override void Verify()
        {
            TryInitializePlayerPref(GetAudioEntryKey(AudioType.Music), 1f);
            TryInitializePlayerPref(GetAudioEntryKey(AudioType.Sfx), 1f);
        }
        
        public void SetVolume(float newVolume, AudioType audioType)
        {
            newVolume = newVolume switch
            {
                < 0f => 0f,
                > 1f => 1f,
                _ => newVolume
            };
            
            PlayerPrefs.SetFloat(GetAudioEntryKey(audioType), newVolume);
        }

        public float GetVolume(AudioType audioType)
        {
            return PlayerPrefs.GetFloat(GetAudioEntryKey(audioType));
        }

        private static string GetAudioEntryKey(AudioType audioType)
        {
            return audioType switch
            {
                AudioType.Sfx => Constants.PlayerPrefKeyParameters.SfxVolume,
                AudioType.Music => Constants.PlayerPrefKeyParameters.MusicVolume,
                _ => throw new ArgumentOutOfRangeException(nameof(audioType), audioType, null)
            };
        }
    }
}