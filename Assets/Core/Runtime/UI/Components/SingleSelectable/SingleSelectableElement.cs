using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.Runtime.UI.Components.SingleSelectable
{
    public sealed class SingleSelectableElement : MonoBehaviour
    {
        [SerializeField] private Button interactable;

        [SerializeField] private UnityEvent onSelected;
        [SerializeField] private UnityEvent onDeselected;

        public Action OnSelectedClick;
        
        private bool _isSelected;
        
        private void Awake()
        {
            interactable.onClick.AddListener(() =>
            {
                OnSelectedClick?.Invoke();
            });
        }

        public void OnSelected(bool isSelected)
        {
            _isSelected = isSelected;
            if (_isSelected)
                onSelected?.Invoke();
            else
                onDeselected?.Invoke();
        }
    }
}