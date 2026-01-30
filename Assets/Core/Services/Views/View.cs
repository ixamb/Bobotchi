using System;
using System.Collections.Generic;
using Core.Extensions;
using UnityEngine;

namespace Core.Services.Views
{
    [RequireComponent(typeof(CanvasGroup), typeof(Animator))]
    public class View : MonoBehaviour, IView
    {
        [SerializeField] protected string viewCode;
        [SerializeField] protected bool useAnimation;
        [SerializeField] private bool keepViewDisplayed;

        public Action OnShow;
        public Action OnHide;
        
        private CanvasGroup _canvasGroup;
        private Animator _animator;

        private readonly List<ViewComponent<ComponentDto>> _spawnedComponents = new();
        
        private static readonly int Show = Animator.StringToHash("Show");
        private static readonly int Hide = Animator.StringToHash("Hide");
        
        protected virtual void Awake()
        {
            ViewService.Instance.RegisterView(this);
            
            _canvasGroup = GetComponent<CanvasGroup>();
            _animator = useAnimation ? GetComponent<Animator>() : null;

            if (keepViewDisplayed)
            {
                TurnCanvasGroupOn();
            }
        }

        public void ShowView()
        {
            if (useAnimation)
            {
                _animator.SetTrigger(Show);
            }
            else
            {
                TurnCanvasGroupOn();
            }
            OnShow?.Invoke();
        }

        public void HideView()
        {
            if (keepViewDisplayed)
                return;
            
            if (useAnimation)
            {
                _animator.SetTrigger(Hide);
            }
            else
            {
                TurnCanvasGroupOff();
            }
            OnHide?.Invoke();
        }

        private void TurnCanvasGroupOn()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        private void TurnCanvasGroupOff()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public bool IsVisibleAndActive()
        {
            return _canvasGroup.alpha > 0
                   && _canvasGroup.interactable
                   && _canvasGroup.blocksRaycasts;
        }

        protected void ClearSpawnedComponents()
        {
            _spawnedComponents.DestroyAndClear();
        }

        protected void AddSpawnedComponent(ViewComponent<ComponentDto> component)
        {
            _spawnedComponents.Add(component);
        }

        public string GetCode() => viewCode;
    }
}