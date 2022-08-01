using tbs.actions;
using tbs.grid;
using UnityEngine;

namespace tbs.units
{
    public class Unit : MonoBehaviour
    {
        private GridPosition _gridPosition;
        public GridPosition GridPosition => _gridPosition;
        
        private MoveAction _moveAction;
        private SpinAction _spinAction;
        public MoveAction MoveAction => _moveAction;
        public SpinAction SpinAction => _spinAction;
        
        private BaseAction[] _baseActionArray;
        public BaseAction[] BaseActionArray => _baseActionArray;


        private void Awake()
        {
            _moveAction = GetComponent<MoveAction>();
            _spinAction = GetComponent<SpinAction>();
            
            _baseActionArray = GetComponents<BaseAction>();

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