﻿using System;
using System.Collections.Generic;
using tbs.grid;
using tbs.units;
using UnityEngine;

namespace tbs.actions
{
    public class ShootAction : BaseAction
    {
        public event Action<Unit, Unit> OnShoot;

        private enum State
        {
            Aiming,
            Shooting,
            CoolOff
        }
        
        private State _state;
        private float _stateTimer;
        
        private Unit _targetUnit;
        
        private readonly int _maxShootDistance  = 7;
        private bool _canShootBullet;


        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            _stateTimer -= Time.deltaTime;

            switch (_state)
            {
                case State.Aiming:
                    Vector3 aimDir = (_targetUnit.GetWorldPosition() - SelectedUnit.GetWorldPosition()).normalized;

                    float rotateSpeed = 10f;
                    transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                    break;
                case State.Shooting:
                    if (_canShootBullet)
                    {
                        Shoot();
                        _canShootBullet = false;
                    }

                    break;
                case State.CoolOff:
                    break;
            }

            if (_stateTimer <= 0f)
            {
                NextState();
            }
        }

        private void NextState()
        {
            switch (_state)
            {
                case State.Aiming:
                    _state = State.Shooting;
                    float shootingStateTime = 0.1f;
                    _stateTimer = shootingStateTime;
                    break;
                case State.Shooting:
                    _state = State.CoolOff;
                    float coolOffStateTime = 0.5f;
                    _stateTimer = coolOffStateTime;
                    break;
                case State.CoolOff:
                    ActionComplete();
                    break;
            }
        }

        private void Shoot()
        {
            OnShoot?.Invoke(SelectedUnit, _targetUnit);
            _targetUnit.Damage(40);
        }

        public override string GetActionName()
        {
            return "Shoot";
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            GridPosition unitGridPosition = SelectedUnit.GridPosition;

            for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
            {
                for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > _maxShootDistance)
                    {
                        continue;
                    }

                    if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                    {
                        // Grid Position is empty, no Unit
                        continue;
                    }

                    Unit unitAtGridPosition = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                    if (unitAtGridPosition.IsEnemy == SelectedUnit.IsEnemy)
                    {
                        // Both Units on same 'team'
                        continue;
                    }

                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

            _state = State.Aiming;
            float aimingStateTime = 1f;
            _stateTimer = aimingStateTime;

            _canShootBullet = true;

            ActionStart(onActionComplete);
        }
        
        public Unit GetTargetUnit()
        {
            return _targetUnit;
        }
        
        public int GetMaxShootDistance()
        {
            return _maxShootDistance;
        }

    }
}