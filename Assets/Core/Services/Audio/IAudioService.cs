using Core.Services.Audio.Dto;

namespace Core.Services.Audio
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