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

        [SerializeField] private GameObject enemyTurnVisualGameObject;

        private void Start()
        {
            endTurnBtn.onClick.AddListener(() =>
            {
                TurnSystem.Instance.NextTurn();
            });

            TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

            UpdateTurnText();
            UpdateEnemyTurnVisual();
            UpdateEndButtonTurnVisual();
        }

        private void TurnSystem_OnTurnChanged()
        {
            UpdateTurnText();
            UpdateEnemyTurnVisual();
            UpdateEndButtonTurnVisual();
        }

        private void UpdateTurnText()
        {
            turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
        }
        
        private void UpdateEnemyTurnVisual()
        {
            enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn);
        }
        
        private void UpdateEndButtonTurnVisual()
        {
            endTurnBtn.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn);
        }

    }
}