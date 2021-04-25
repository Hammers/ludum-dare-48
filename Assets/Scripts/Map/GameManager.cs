    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        public event Action NewSessionStarted;
        
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
            foreach (var terminal in _terminalsActivatedThisSession)
            {
                terminal.Reset();
            }
            _terminalsActivatedThisSession.Clear();
            NewSessionStarted?.Invoke();
        }

        public void AddUsedTerminal(Terminal terminal)
        {
            _terminalsActivatedThisSession.Add(terminal);
        }

        public void EndSession()
        {
            _terminalsActivatedThisSession.Clear();
            NewSessionStarted?.Invoke();
        }
    }
