using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Utils
{
    public sealed class ScrollToElement : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect; 
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private float scrollSpeed = 10f; 

        public void ScrollTo(RectTransform targetElement)
        {
            if (scrollSpeed <= 0.01f)
            {
                SetScrollPosition(targetElement);
            }
            else
            {
                StartCoroutine(SmoothScrollTo(targetElement));
            }
        }

        private void SetScrollPosition(RectTransform targetElement)
        {
            var targetLocalPosition = (Vector2)scrollRect.content.localPosition - (Vector2)targetElement.localPosition;
            var normalizedY = targetLocalPosition.y / (contentPanel.sizeDelta.y - scrollRect.viewport.rect.height);
            scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.x, normalizedY);
        }
        
        private IEnumerator SmoothScrollTo(RectTransform targetElement)
        {
            var targetLocalPosition = (Vector2)scrollRect.content.localPosition - (Vector2)targetElement.localPosition;
            var targetNormalizedY = targetLocalPosition.y / (contentPanel.sizeDelta.y - scrollRect.viewport.rect.height);
            targetNormalizedY = Mathf.Clamp01(targetNormalizedY);

            while (Mathf.Abs(scrollRect.normalizedPosition.y - targetNormalizedY) > 0.001f)
            {
                var newY = Mathf.Lerp(scrollRect.normalizedPosition.y, targetNormalizedY, Time.deltaTime * scrollSpeed);
                scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.x, newY);
                
                yield return null;
            }
            
            scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.x, targetNormalizedY);
        }
    }
}