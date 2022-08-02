using System;
using tbs.actions;
using tbs.grid;
using UnityEngine;
using UnityEngine.EventSystems;

namespace tbs.units
{
    public class UnitActionSystem : MonoBehaviour
    {
        public static UnitActionSystem Instance { get; private set; }

        [SerializeField] private Unit _selectedUnit;
        public Unit SelectedUnit => _selectedUnit;

        [SerializeField] private LayerMask _unitLayerMask;

        private BaseAction _selectedAction;
        private bool _isBusy;

        public event Action<Unit> OnSelectedUnitChanged;
        public event Action<BaseAction> OnSelectedActionChanged;
        public event Action<bool> OnBusyChanged;

        public event Action OnActionStarted;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There is more than one instance: " + transform);
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            SetSelectedUnit(_selectedUnit);
        }

        private void Update()
        {
            if (_isBusy)
            {
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (TryHandleUnitSelection())
            {
                return;
            }

            HandleSelectedAction();
        }

        private void SetBusy()
        {
            _isBusy = true;
            OnBusyChanged?.Invoke(_isBusy);
        }

        private void ClearBusy()
        {
            _isBusy = false;
            OnBusyChanged?.Invoke(_isBusy);
        }


        private bool TryHandleUnitSelection()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask))
                {
                    if (raycastHit.collider.TryGetComponent<Unit>(out Unit unit))
                    {
                        if (unit == _selectedUnit)
                        {
                            // Unit is already selected
                            return false;
                        }

                        SetSelectedUnit(unit);
                        return true;
                    }
                }
            }

            return false;
        }

        private void HandleSelectedAction()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

                if (!_selectedAction.IsValidActionGridPosition(mouseGridPosition))
                    return;

                if (!_selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction))
                    return;

                SetBusy();
                _selectedAction.TakeAction(mouseGridPosition, ClearBusy);
                
                OnActionStarted?.Invoke();
            }
        }


        private void SetSelectedUnit(Unit unit)
        {
            _selectedUnit = unit;

            SetSelectedAction(unit.MoveAction);

            OnSelectedUnitChanged?.Invoke(_selectedUnit);
        }

        public BaseAction GetSelectedAction()
        {
            return _selectedAction;
        }

        public void SetSelectedAction(BaseAction baseAction)
        {
            _selectedAction = baseAction;
            OnSelectedActionChanged?.Invoke(_selectedAction);
        }
    }
}