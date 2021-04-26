    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        public event Action NewSessionStarted;
        public event Action<CoinTerminal> CoinTerminalActivated;
        
        private static GameManager _instance;
        public static GameManager Instance => _instance;

        private List<Terminal> _terminalsActivatedThisSession = new List<Terminal>();
        
        
        public void Awake()
        {
            if (_instance != null)
            {
                Debug.LogError("2 Game Managers in scene!");
            }

            _instance = this;
        }


        public void ResetSession()
        {
            FindObjectOfType<ShopPointer>()?.Hide();
            foreach (var terminal in _terminalsActivatedThisSession)
            {
                terminal.Reset();
            }
            _terminalsActivatedThisSession.Clear();
            NewSessionStarted?.Invoke();
        }

        public void ActivateCoinTerminal(CoinTerminal terminal)
        {
            CoinTerminalActivated?.Invoke(terminal);
        }
        
        public void AddUsedTerminal(Terminal terminal)
        {
            FindObjectOfType<ShopPointer>()?.Show();
            _terminalsActivatedThisSession.Add(terminal);
        }

        public void EndSession()
        {
            FindObjectOfType<ShopPointer>()?.Hide();
            _terminalsActivatedThisSession.Clear();
        }
    }
