using System;
using System.Collections.Generic;
using tbs.enemyAI;
using tbs.grid;
using UnityEngine;

namespace tbs.actions
{
    public class MoveAction : BaseAction
    {
        [SerializeField] private float _stoppingDistance = 0.5f;
        [SerializeField] private float _moveSpeed = 4f;
        [SerializeField] private float _rotateSpeed = 10f;

        [SerializeField] private int _maxMoveDistance = 6;

        private Vector3 _targetPosition;

        public event Action OnStartMoving;
        public event Action OnStopMoving;

        private List<Vector3> positionList;
        private int currentPositionIndex;

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            Vector3 targetPosition = positionList[currentPositionIndex];
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);

            if (Vector3.Distance(_targetPosition, transform.position) > _stoppingDistance)
            {
                transform.position += moveDirection * (Time.deltaTime * _moveSpeed);
            }
            else
            {
                currentPositionIndex++;
                if (currentPositionIndex >= positionList.Count)
                {
                    OnStopMoving?.Invoke();

                    ActionComplete();
                }
            }
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            List<GridPosition> pathGridPositionList =
                Pathfinder.Instance.FindPath(SelectedUnit.GridPosition, gridPosition, out int pathLength);

            currentPositionIndex = 0;
            positionList = new List<Vector3>();

            // Transform grid positions into world positions for movement
            foreach (GridPosition pathGridPosition in pathGridPositionList)
            {
                positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
            }

            OnStartMoving?.Invoke();
            ActionStart(onActionComplete);
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

                    if (!Pathfinder.Instance.IsWalkableGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    if (!Pathfinder.Instance.HasPath(unitGridPosition, testGridPosition))
                    {
                        continue;
                    }

                    int pathfindingDistanceMultiplier = 10;
                    if (Pathfinder.Instance.GetPathLength(unitGridPosition, testGridPosition) >
                        _maxMoveDistance * pathfindingDistanceMultiplier)
                    {
                        // Path length is too long
                        continue;
                    }

                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }


        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            int targetCountAtGridPosition = SelectedUnit
                .GetAction<ShootAction>()
                .GetTargetCountAtPosition(gridPosition);
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = targetCountAtGridPosition * 10,
            };
        }


        public override string GetActionName()
        {
            return "Move";
        }
    }
}