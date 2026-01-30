using System.Collections.Generic;
using System.Linq;
using Core.Services.Audio.Dto;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.Services.Audio
{
    internal sealed class AudioService : Singleton<AudioService, IAudioService>, IAudioService
    {
        [SerializeField] private AudioServiceProperties properties;
        
        private readonly List<AudioSourceEntry> _audioSourceEntries = new();

        protected override void Init()
        {
            var audioObject = Instantiate(new GameObject("AudioObject"), transform);
            for (uint i = 0; i < properties.CanalsCount; i++)
            {
                var source = audioObject.AddComponent<AudioSource>();
                _audioSourceEntries.Add(new AudioSourceEntry(source));
            }
        }

        public void LoadAudio(AudioLoadDto audioLoadDto)
        {
            var clipEntry = properties.GetAudioClipEntry(audioLoadDto.Code);
            if (clipEntry?.AudioClip is null)
                return;
            
            foreach (var entry in _audioSourceEntries.Where(entry => !entry.IsPlaying()))
            {
                entry.PlayAudio(new AudioPlayDto(clipEntry.AudioType, clipEntry.AudioClip, audioLoadDto));
                break;
            };
        }

        public void PauseAudio(string code) => GetPlayingAudioSourceEntry(code)?.PauseAudio();
        public void ResumeAudio(string code) => GetPlayingAudioSourceEntry(code)?.ResumeAudio();
        public void StopAudio(string code) => GetPlayingAudioSourceEntry(code)?.StopAudio();
        public void StopAllAudios() => _audioSourceEntries.ForEach(entry => entry.StopAudio());
        public void RestartAudio(string code) => GetPlayingAudioSourceEntry(code)?.RestartAudio();

        public void ChangeVolume(AudioType audioType, float volume)
        {
            volume = _audioSourceEntries.Where(entry => entry.AudioType == audioType)
                .Aggregate(volume, (current, entry) => Mathf.Clamp01(current));
        }

        public void ChangeVolume(string code, float volume) => GetPlayingAudioSourceEntry(code)?.ChangeVolume(volume);
                
        [CanBeNull] private AudioSourceEntry GetPlayingAudioSourceEntry(string code)
            => _audioSourceEntries.FirstOrDefault(entry => entry.Code.Equals(code));
    }
}