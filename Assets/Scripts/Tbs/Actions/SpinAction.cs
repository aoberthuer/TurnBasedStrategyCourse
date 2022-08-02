using System;
using System.Collections.Generic;
using tbs.grid;
using UnityEngine;

namespace tbs.actions
{
    public class SpinAction : BaseAction
    {
        private float _totalSpinAmount;

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }
        
            float spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

            _totalSpinAmount += spinAddAmount;
            if (_totalSpinAmount >= 360f)
            {
                IsActive = false;
                OnActionComplete();

            }
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            OnActionComplete = onActionComplete;
            IsActive = true;
            
            _totalSpinAmount = 0f;
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            GridPosition unitGridPosition = SelectedUnit.GridPosition;

            return new List<GridPosition>
            {
                unitGridPosition
            };
        }
        
        public override string GetActionName()
        {
            return "Spin";
        }

    }
}