using Core.Runtime.Services.Audio.Dto;
using Core.Runtime.Services;

namespace Core.Runtime.Services.Audio
{
    public interface IAudioService : ISingleton
    {
        void LoadAudio(AudioLoadDto audioLoadDto);
        void PauseAudio(string code);
        void ResumeAudio(string code);
        void StopAudio(string code);
        void StopAllAudios();
        void RestartAudio(string code);
        void ChangeVolume(AudioType audioType, float volume);
        void ChangeVolume(string code, float volume);
    }
}