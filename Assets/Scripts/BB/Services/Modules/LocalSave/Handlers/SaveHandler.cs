namespace BB.Services.Modules.LocalSave.Handlers
{
    public abstract class SaveHandler<T> where T : SaveDto, new()
    {
        protected abstract string ConstantEntryKey { get; }
        
        private BBLocalSaveService _localSaveService;
        
        internal SaveHandler<T> Bind(BBLocalSaveService localSaveService)
        {
            _localSaveService = localSaveService;
            return this;
        }
        
        internal SaveHandler<T> Verify()
        {
            if (_localSaveService.Get<T>(ConstantEntryKey) is null)
                _localSaveService.Set(ConstantEntryKey, new T(), autoSave: true);
            return this;
        }
        
        protected T GetData(T defaultValue = null) => _localSaveService.Get(ConstantEntryKey, defaultValue) ?? new T();
        protected void SetData(T value, bool autoSave = false) => _localSaveService.Set(ConstantEntryKey, value, autoSave);
    }
}