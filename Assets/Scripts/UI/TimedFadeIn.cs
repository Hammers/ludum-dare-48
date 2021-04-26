using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class TimedFadeIn : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        public float delay;
        public float duration;
        
        void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
            _canvasGroup.DOFade(1f, duration).SetDelay(delay);
        }
        
    }
}