    using System;
    using DG.Tweening;
    using UnityEngine;

    public class DeasthScreen : MonoBehaviour
    {
        private CharacterDeath _characterDeath;

        [SerializeField] private CanvasGroup _canvasGroup;
        private void Start()
        {
            _canvasGroup.alpha = 0f;
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
        }

        private void OnDeath()
        {
            _canvasGroup.DOFade(1f, 0.5f);
        }

        public void PressButton()
        {
            GameManager.Instance.ResetSession();
        }
    }
