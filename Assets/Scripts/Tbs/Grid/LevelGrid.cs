﻿using System;
using System.Collections.Generic;
using tbs.units;
using UnityEngine;
using UnityEngine.Serialization;

namespace tbs.grid
{
    public class LevelGrid : MonoBehaviour
    {
        public static LevelGrid Instance { get; private set; }
        
        [SerializeField] private int _width = 10;
        [SerializeField] private int _height = 10;
        [SerializeField] private float _cellSize = 2;


        [FormerlySerializedAs("gridDebugObjectPrefab")] [SerializeField] private Transform _gridDebugObjectPrefab;

        private GridSystem<GridObject> _gridSystem;
        public event Action OnAnyUnitMovedGridPosition;


        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There's more than one LevelGrid! " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }

            Instance = this;

            _gridSystem = new GridSystem<GridObject>(_width, _height, _cellSize,
                (GridSystem<GridObject> gridSystem, GridPosition gridPosition) =>
                    new GridObject(gridSystem, gridPosition));

            // _gridSystem.CreateDebugObjects(transform, gridDebugObjectPrefab); // TODO: Disabled, because enabled on Pathfinding
        }
        
        private void Start()
        {
            Pathfinder.Instance.Setup(_width, _height, _cellSize);
        }



        public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
        {
            GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            gridObject.AddUnit(unit);
        }

        public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
        {
            GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            return gridObject.GetUnitList();
        }

        public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
        {
            GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            gridObject.RemoveUnit(unit);
        }

        public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
        {
            RemoveUnitAtGridPosition(fromGridPosition, unit);
            AddUnitAtGridPosition(toGridPosition, unit);

            OnAnyUnitMovedGridPosition?.Invoke();
        }

        public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);

        public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);

        public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);

        public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
        {
            GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            return gridObject.HasAnyUnit();
        }

        public Unit GetUnitAtGridPosition(GridPosition gridPosition)
        {
            GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            return gridObject.GetUnit();
        }


        public int GetWidth()
        {
            return _gridSystem.Width;
        }

        public int GetHeight()
        {
            return _gridSystem.Height;
        }
    }
}