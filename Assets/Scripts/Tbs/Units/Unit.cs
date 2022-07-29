using tbs.grid;
using UnityEngine;

namespace tbs.units
{
    public class Unit : MonoBehaviour
    {
        private GridPosition _gridPosition;
        public GridPosition GridPosition => _gridPosition;
        
        private MoveAction _moveAction;
        public MoveAction MoveAction => _moveAction;

        private void Awake()
        {
            _moveAction = GetComponent<MoveAction>();
        }

        private void Start()
        {
            _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
        }


        private void Update()
        {
            GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            if (newGridPosition != _gridPosition)
            {
                // Unit changed Grid Position
                LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
                _gridPosition = newGridPosition;
            }
        }

    }
}