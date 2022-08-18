using System;
using System.Collections.Generic;
using UnityEngine;

namespace tbs.units
{
    public class UnitManager : MonoBehaviour
    {
        public static UnitManager Instance { get; private set; }


        private readonly List<Unit> _unitList = new List<Unit>();
        private readonly List<Unit> _friendlyUnitList = new List<Unit>();
        private readonly List<Unit> _enemyUnitList = new List<Unit>();


        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There's more than one UnitManager! " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
            Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
        }

        private void Unit_OnAnyUnitSpawned(Unit unit)
        {
            Debug.Log($"Added {unit}");
            
            _unitList.Add(unit);
            if (unit.IsEnemy)
            {
                _enemyUnitList.Add(unit);
            }
            else
            {
                _friendlyUnitList.Add(unit);
            }
        }

        private void Unit_OnAnyUnitDead(Unit unit)
        {
            _unitList.Remove(unit);
            if (unit.IsEnemy)
            {
                _enemyUnitList.Remove(unit);
            }
            else
            {
                _friendlyUnitList.Remove(unit);
            }
        }

        public List<Unit> GetUnitList()
        {
            return _unitList;
        }

        public List<Unit> GetFriendlyUnitList()
        {
            return _friendlyUnitList;
        }

        public List<Unit> GetEnemyUnitList()
        {
            return _enemyUnitList;
        }
    }
}