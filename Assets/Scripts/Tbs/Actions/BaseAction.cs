using System;
using System.Collections.Generic;
using tbs.enemyAI;
using tbs.grid;
using tbs.units;
using UnityEngine;

namespace tbs.actions
{
    public abstract class BaseAction : MonoBehaviour
    {
        protected Unit SelectedUnit;

        protected bool IsActive;
        private Action _onActionComplete;

        public static event Action<BaseAction> OnAnyActionStarted;
        public static event Action<BaseAction> OnAnyActionCompleted;


        protected virtual void Awake()
        {
            SelectedUnit = GetComponent<Unit>();
        }

        public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

        public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
        {
            List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
            return validGridPositionList.Contains(gridPosition);
        }

        public abstract List<GridPosition> GetValidActionGridPositionList();

        protected void ActionStart(Action onActionComplete)
        {
            IsActive = true;
            _onActionComplete = onActionComplete;

            OnAnyActionStarted?.Invoke(this);
        }

        protected void ActionComplete()
        {
            IsActive = false;
            _onActionComplete();

            OnAnyActionCompleted?.Invoke(this);
        }

        public virtual int GetActionPointsCost()
        {
            return 1;
        }

        public EnemyAIAction GetBestEnemyAIAction()
        {
            List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

            List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

            foreach (GridPosition gridPosition in validActionGridPositionList)
            {
                EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
                enemyAIActionList.Add(enemyAIAction);
            }

            if (enemyAIActionList.Count > 0)
            {
                enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
                return enemyAIActionList[0];
            }
            else
            {
                // No possible Enemy AI Actions
                return null;
            }
        }

        public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);


        public abstract string GetActionName();
    }
}