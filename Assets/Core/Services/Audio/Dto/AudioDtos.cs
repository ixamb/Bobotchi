using UnityEngine;

namespace Core.Services.Audio.Dto
{
    public record AudioLoadDto(string Code, bool Loop, bool AutoPlay = true, float Volume = 0.5f)
    {
        public string Code { get; } = Code;
        public bool Loop { get; } = Loop;
        public bool AutoPlay { get; } = AutoPlay;
        public float Volume { get; } = Volume;
    }

    public record AudioPlayDto(AudioType AudioType, AudioClip Clip, bool Loop, string Code, bool AutoPlay = true, float Volume = 0.5f)
    {
        public AudioType AudioType { get; private set; } = AudioType;
        public AudioClip Clip { get; private set; } = Clip;
        public float Volume { get; private set; } = Volume;
        public bool Loop { get; private set; } = Loop;
        public string Code { get; private set; } = Code;
        public bool AutoPlay { get; private set; } = AutoPlay;

        public AudioPlayDto(AudioType audioType, AudioClip clip, AudioLoadDto audioLoadDto)
            : this(audioType, clip, audioLoadDto.Loop, audioLoadDto.Code, audioLoadDto.AutoPlay, audioLoadDto.Volume)
        {
        }
    }
}