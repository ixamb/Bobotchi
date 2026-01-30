namespace BB.Services.Modules.LocalSave
{
    // ReSharper disable once InconsistentNaming
    public interface BBSetLocalSaveObserver
    {
        void OnLocalSaved<T>(string key, T value);
    }
}