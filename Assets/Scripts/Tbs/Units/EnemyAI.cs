using System;
using tbs.actions;
using tbs.grid;
using tbs.turns;
using UnityEngine;

namespace tbs.units
{
    public class EnemyAI : MonoBehaviour
    {
        private enum State
        {
            WAITING_FOR_ENEMY_TURN,
            TAKING_TURN,
            BUSY,
        }

        private State _state;
        
        private float _timer;

        private void Awake()
        {
            _state = State.WAITING_FOR_ENEMY_TURN;
        }



        private void Start()
        {
            TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        }

        private void Update()
        {
            if (TurnSystem.Instance.IsPlayerTurn)
            {
                return;
            }

            switch (_state)
            {
                case State.WAITING_FOR_ENEMY_TURN:
                    break;
                case State.TAKING_TURN:
                    _timer -= Time.deltaTime;
                    if (_timer <= 0f)
                    {
                        if (TryTakeEnemyAIAction(SetStateTakingTurn))
                        {
                            _state = State.BUSY;
                        } else
                        {
                            // No more enemies have actions they can take. End enemy turn!
                            TurnSystem.Instance.NextTurn();
                        }
                    }
                    break;
                case State.BUSY:
                    break;
            }

        }

        private void SetStateTakingTurn()
        {
            _timer = 0.5f;
            _state = State.TAKING_TURN;
        }

        private void TurnSystem_OnTurnChanged()
        {
            if (!TurnSystem.Instance.IsPlayerTurn)
            {
                _state = State.TAKING_TURN;
                _timer = 2f;
            }
        }

        private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
        {
            Debug.Log("Take Enemy AI Action");
            foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
            {
                if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
        {
            SpinAction spinAction = enemyUnit.SpinAction;
            GridPosition actionGridPosition = enemyUnit.GridPosition;

            if (!spinAction.IsValidActionGridPosition(actionGridPosition))
            {
                return false;
            }

            if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
            {
                return false;
            }

            spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
            return true;
        }

    }
}