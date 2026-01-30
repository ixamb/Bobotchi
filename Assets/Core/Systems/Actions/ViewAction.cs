using System;
using Core.Services.Views;
using UnityEngine;

namespace Core.Systems.Actions
{
    [CreateAssetMenu(fileName = "View Action", menuName = "Core/Systems/Actions/View Action")]
    public sealed class ViewAction : GameAction
    {
        private enum ViewActionType { Show, Hide }
        
        [SerializeField] private ViewActionType viewActionType;
        [SerializeField] private string viewCode;
        
        protected override void Executable()
        {
            var view = ViewService.Instance.GetView(viewCode);
            if (view is null)
                return;

            switch (viewActionType)
            {
                case ViewActionType.Show: view.ShowView(); break;
                case ViewActionType.Hide: view.HideView(); break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}