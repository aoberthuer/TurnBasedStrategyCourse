using System;
using tbs.turns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace tbs.ui
{
    public class TurnSystemUI : MonoBehaviour
    {
        [SerializeField] private Button endTurnBtn;
        [SerializeField] private TextMeshProUGUI turnNumberText;

        private void Start()
        {
            endTurnBtn.onClick.AddListener(() =>
            {
                TurnSystem.Instance.NextTurn();
            });

            TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

            UpdateTurnText();
        }

        private void TurnSystem_OnTurnChanged()
        {
            UpdateTurnText();
        }

        private void UpdateTurnText()
        {
            turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
        }

    }
}