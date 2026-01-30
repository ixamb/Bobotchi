using Core.Services.Audio.Dto;
using UnityEngine;

namespace Core.Services.Audio
{
    internal sealed class AudioSourceEntry
    {
        internal AudioType AudioType { get; private set; }
        private readonly AudioSource _source;

        internal string Code { get; private set; }
        
        internal AudioSourceEntry(AudioSource source)
        {
            _source = source;
        }

        internal void PlayAudio(AudioPlayDto audioPlayDto)
        {
            AudioType = audioPlayDto.AudioType;
            _source.clip = audioPlayDto.Clip;
            _source.loop = audioPlayDto.Loop;
            _source.volume = audioPlayDto.Volume;
            Code = audioPlayDto.Code;
            if (audioPlayDto.AutoPlay)
                _source.Play();
        }

        internal void PauseAudio()
        {
            _source.Pause();
        }

        internal void ResumeAudio()
        {
            _source.UnPause();
        }

        internal void RestartAudio()
        {
            _source.Stop();
            _source.Play();
        }

        internal void StopAudio()
        {
            _source.Stop();
        }

        internal void ChangeVolume(float newVolume)
        {
            _source.volume = newVolume;
        }
        
        internal bool IsPlaying() => _source.isPlaying;
    }
}