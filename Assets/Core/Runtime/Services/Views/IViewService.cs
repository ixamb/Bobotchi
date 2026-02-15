namespace Core.Runtime.Services.Views
{
    public interface IViewService : ISingleton
    {
        void RegisterView(IView view);
        void UnregisterView(IView view);
        IView GetView(string code);
        TIView GetView<TIView>(string code) where TIView : class, IView;
    }
}