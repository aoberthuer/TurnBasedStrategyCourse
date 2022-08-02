using System;
using System.Collections.Generic;
using tbs.grid;
using tbs.units;
using UnityEngine;

namespace tbs.actions
{
    public class MoveAction : BaseAction
    {
        [SerializeField] private float _stoppingDistance = 0.5f;
        [SerializeField] private float _moveSpeed = 4f;
        [SerializeField] private float _rotateSpeed = 10f;
        
        [SerializeField] private int _maxMoveDistance = 4;

        
        private Vector3 _targetPosition;

        private Animator _unitAnimator;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        
        protected override void Awake()
        {
            base.Awake();

            _unitAnimator = GetComponentInChildren<Animator>();
            _targetPosition = transform.position;
        }

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            if (Vector3.Distance(_targetPosition, transform.position) > _stoppingDistance)
            {
                
                transform.position += moveDirection * (Time.deltaTime * _moveSpeed);
                _unitAnimator.SetBool(IsWalking, true);
            }
            else
            {
                _unitAnimator.SetBool(IsWalking, false);
                
                IsActive = false;
                OnActionComplete();
            }
            
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
        }
        
        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            OnActionComplete = onActionComplete;
            _targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
            
            IsActive = true;
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            GridPosition unitGridPosition = SelectedUnit.GridPosition;

            for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
            {
                for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    if (unitGridPosition == testGridPosition)
                    {
                        // Same Grid Position where the unit is already at
                        continue;
                    }

                    if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    {
                        // Grid Position already occupied with another Unit
                        continue;
                    }

                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }
        
        public override string GetActionName()
        {
            return "Move";
        }

    }
}