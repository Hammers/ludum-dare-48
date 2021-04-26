    using System;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;

    public class DeasthScreen : MonoBehaviour
    {
        private CharacterDeath _characterDeath;

        [SerializeField] private CanvasGroup _canvasGroup;
        private void Start()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _characterDeath = FindObjectOfType<CharacterDeath>();
            _characterDeath.Died += OnDeath;
        }

        private void OnEnable()
        {
            GameManager.Instance.NewSessionStarted += OnNewSession;
        }

        private void OnDisable()
        {
            GameManager.Instance.NewSessionStarted -= OnNewSession;
        }

        private void OnNewSession()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private void OnDeath()
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.DOFade(1f, 0.5f).onKill = () =>
            {
                _canvasGroup.interactable = true;
            };
        }

        public void PressButton()
        {
            GameManager.Instance.ResetSession();
        }
    }
