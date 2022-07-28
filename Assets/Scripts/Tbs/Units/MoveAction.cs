using System;
using System.Collections.Generic;
using tbs.grid;
using UnityEngine;

namespace tbs.units
{
    public class MoveAction : MonoBehaviour
    {
        [SerializeField] private float _stoppingDistance = 0.5f;
        [SerializeField] private float _moveSpeed = 4f;
        [SerializeField] private float _rotateSpeed = 10f;
        
        [SerializeField] private int _maxMoveDistance = 4;

        
        private Vector3 _targetPosition;

        private Unit _unit;
        private Animator _unitAnimator;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        
        private void Awake()
        {
            _unit = GetComponent<Unit>();
            _unitAnimator = GetComponentInChildren<Animator>();

            _targetPosition = transform.position;
        }

        private void Update()
        {
            if (Vector3.Distance(_targetPosition, transform.position) > _stoppingDistance)
            {
                Vector3 moveDirection = (_targetPosition - transform.position).normalized;
                transform.position += moveDirection * (Time.deltaTime * _moveSpeed);

                // transform.forward = moveDirection; without interpolation
                transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);

                _unitAnimator.SetBool(IsWalking, true);
            }
            else
            {
                _unitAnimator.SetBool(IsWalking, false);
            }
        }
        
        public void Move(GridPosition gridPosition)
        {
            _targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        }
        
        public bool IsValidActionGridPosition(GridPosition gridPosition)
        {
            List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
            return validGridPositionList.Contains(gridPosition);
        }

        public List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            GridPosition unitGridPosition = _unit.GridPosition;

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

    }
}