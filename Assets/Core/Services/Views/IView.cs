namespace Core.Services.Views
{
    public interface IView
    {
        void ShowView();
        void HideView();
        string GetCode();
        bool IsVisibleAndActive();
    }
}