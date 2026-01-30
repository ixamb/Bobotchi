namespace Core.Services.LocalSave
{
    public interface ILocalSaveService : ISingleton
    {
        void Set<T>(string key, T value, bool autoSave = true);
        T Get<T>(string key, T defaultValue = default);
        void Save();
        void Delete();
    }
}