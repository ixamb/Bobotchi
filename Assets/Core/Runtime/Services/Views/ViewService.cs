using System.Collections.Generic;
using System.Linq;

namespace Core.Runtime.Services.Views
{
    public sealed class ViewService : Singleton<ViewService, IViewService>, IViewService
    {
        private readonly Dictionary<string, IView> _views = new();

        protected override void Init()
        {
        }

        public void RegisterView(IView view)
        {
            var viewCode = view.GetCode();
            if (string.IsNullOrWhiteSpace(viewCode))
                return;
            
            _views.TryAdd(viewCode, view);
        }

        public void UnregisterView(IView view)
        {
            var viewCode = view.GetCode();
            if (string.IsNullOrWhiteSpace(viewCode))
                return;
            
            _views.Remove(viewCode);
        }

        public IEnumerable<IView> GetActiveAndEnabledViews()
        {
            return _views.Values.Where(view => view.IsVisibleAndActive());
        }

        public IView GetView(string code)
        {
            return _views.GetValueOrDefault(code);
        }

        public TIView GetView<TIView>(string code) where TIView : class, IView
        {
            if (_views.TryGetValue(code, out var view))
            {
                return view as TIView;
            }
            return null;
        }
    }
}