using System;
using System.Collections.Generic;
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
        }

        protected void ActionComplete()
        {
            IsActive = false;
            _onActionComplete();
        }

        public virtual int GetActionPointsCost()
        {
            return 1;
        }

        public abstract string GetActionName();
    }
}