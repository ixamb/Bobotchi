using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.UI.Components.SingleSelectable
{
    public sealed class SingleSelectableGroup : MonoBehaviour
    {
        [SerializeField] private InitializationMode initializationMode = InitializationMode.Manual;
        [SerializeField] private Transform groupContainer;
        
        public SingleSelectableElement SelectedElement { get; private set; }
        public List<SingleSelectableElement> SelectableElements { get; private set; } = new();
        
        private void Awake()
        {
            if (initializationMode == InitializationMode.Awake)
                Initialize();
        }

        private void Start()
        {
            if (initializationMode == InitializationMode.Start)
                Initialize();
        }

        public void Initialize()
        {
            SelectableElements = groupContainer.GetComponentsInChildren<SingleSelectableElement>().ToList();
            
            SelectableElements.ForEach(element =>
            {
                element.OnSelectedClick += () =>
                {
                    SelectElementAndUnselectRemaining(element, SelectableElements);
                    SelectedElement = element;
                };
            });
        }
        
        public Transform GroupContainer => groupContainer;

        private static void SelectElementAndUnselectRemaining(SingleSelectableElement selectedElement, List<SingleSelectableElement> unselectableElements)
        {
            unselectableElements.ForEach(element => { element.OnSelected(element == selectedElement); });
        }

        private enum InitializationMode
        {
            Awake,
            Start,
            Manual,
        }
    }
}