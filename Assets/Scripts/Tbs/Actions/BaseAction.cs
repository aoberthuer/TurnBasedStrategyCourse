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
        protected Action OnActionComplete;

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
        
        
        public abstract string GetActionName();
    }
}