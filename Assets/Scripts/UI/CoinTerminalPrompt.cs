using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinTerminalPrompt : MonoBehaviour
{
     [SerializeField] CanvasGroup _canvasGroup;

     [SerializeField] TextMeshProUGUI _debuffText;
     [SerializeField] TextMeshProUGUI _coinText;
     private CoinTerminal _currentTerminal;

     private void Start()
     {
          _canvasGroup.alpha = 0f;
          _canvasGroup.interactable = false;
          _canvasGroup.blocksRaycasts = false;
     }
     
     private void OnEnable()
     {
          GameManager.Instance.CoinTerminalActivated += OnTerminalActivated;
     }

     private void OnDisable()
     {
          GameManager.Instance.CoinTerminalActivated -= OnTerminalActivated;
     }
     
     private void OnTerminalActivated(CoinTerminal terminal)
     {
          _canvasGroup.interactable = true;
          _canvasGroup.blocksRaycasts = true;
          _canvasGroup.alpha = 0f;
          _currentTerminal = terminal;
          _canvasGroup.DOFade(1f, 0.3f);
          _coinText.text = terminal.Coins.ToString();
     }

     public void Cancel()
     {
          _canvasGroup.DOFade(0f, 0.3f).onComplete = () =>
          {
               _canvasGroup.interactable = false;
               _canvasGroup.blocksRaycasts = false;
          };
          _currentTerminal.CancelActivation();
     }
     
     
     public void Accept()
     {
          _canvasGroup.DOFade(0f, 0.3f).onComplete = () => 
          {
               _canvasGroup.interactable = false;
               _canvasGroup.blocksRaycasts = false;
          };
          _currentTerminal.AcceptActivation();
     }
}
