using System;
using tbs.actions;
using tbs.grid;
using tbs.turns;
using UnityEngine;

namespace tbs.units
{
    public class Unit : MonoBehaviour
    {
        private const int ACTION_POINTS_MAX = 2;
        public static event Action OnAnyActionPointsChanged;

        [SerializeField] private bool isEnemy;
        public bool IsEnemy => isEnemy;
        
        private GridPosition _gridPosition;
        public GridPosition GridPosition => _gridPosition;

        private HealthSystem _healthSystem;
        private MoveAction _moveAction;
        private SpinAction _spinAction;
        public MoveAction MoveAction => _moveAction;
        public SpinAction SpinAction => _spinAction;

        private BaseAction[] _baseActionArray;
        public BaseAction[] BaseActionArray => _baseActionArray;

        private int _actionPoints = ACTION_POINTS_MAX;


        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();
            
            _moveAction = GetComponent<MoveAction>();
            _spinAction = GetComponent<SpinAction>();

            _baseActionArray = GetComponents<BaseAction>();
        }

        private void Start()
        {
            _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
            
            _healthSystem.OnDead += HealthSystem_OnDead;
            
            TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        }

        private void OnDisable()
        {
            _healthSystem.OnDead -= HealthSystem_OnDead;
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
        
        private void HealthSystem_OnDead()
        {
            LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);
            Destroy(gameObject);
        }

        public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
        {
            if (CanSpendActionPointsToTakeAction(baseAction))
            {
                SpendActionPoints(baseAction.GetActionPointsCost());
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
        {
            if (_actionPoints >= baseAction.GetActionPointsCost())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SpendActionPoints(int amount)
        {
            _actionPoints -= amount;
            OnAnyActionPointsChanged?.Invoke();
        }

        public int GetActionPoints()
        {
            return _actionPoints;
        }
        
        private void TurnSystem_OnTurnChanged()
        {
            if ((isEnemy && !TurnSystem.Instance.IsPlayerTurn) ||
                (!isEnemy && TurnSystem.Instance.IsPlayerTurn))
            {
                _actionPoints = ACTION_POINTS_MAX;
                OnAnyActionPointsChanged?.Invoke();
            }
        }
        
        public Vector3 GetWorldPosition()
        {
            return transform.position;
        }
        
        public void Damage(int damageAmount)
        {
            _healthSystem.Damage(damageAmount);
        }

    }
}