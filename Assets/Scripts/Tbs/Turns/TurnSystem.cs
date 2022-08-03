using System;
using UnityEngine;

namespace tbs.turns
{
    public class TurnSystem : MonoBehaviour
    {
        public static TurnSystem Instance { get; private set; }
        
        public Action OnTurnChanged;


        private int _turnNumber = 1;
        
        private bool _isPlayerTurn = true;
        public bool IsPlayerTurn => _isPlayerTurn;


        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There's more than one TurnSystem! " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }


        public void NextTurn()
        {
            _turnNumber++;
            _isPlayerTurn = !_isPlayerTurn;

            OnTurnChanged?.Invoke();
        }

        public int GetTurnNumber()
        {
            return _turnNumber;
        }

    }
}