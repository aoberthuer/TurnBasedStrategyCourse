using System;
using UnityEngine;

namespace tbs.turns
{
    public class TurnSystem : MonoBehaviour
    {
        public static TurnSystem Instance { get; private set; }
        
        public Action OnTurnChanged;


        private int turnNumber = 1;


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
            turnNumber++;

            OnTurnChanged?.Invoke();
        }

        public int GetTurnNumber()
        {
            return turnNumber;
        }

    }
}