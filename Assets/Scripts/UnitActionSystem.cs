using System;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    
    [SerializeField] private Unit _selectedUnit;
    public Unit SelectedUnit => _selectedUnit;

    [SerializeField] private LayerMask _unitLayerMask;

    public event Action<Unit> OnSelectedUnitChanged;

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection())
                return;

            _selectedUnit.Move(MouseWorld.GetPosition());
        }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask))
        {
            // Unit unit = raycastHit.collider.GetComponent<Unit>();
            // if (unit != null) would work as well!
            if (raycastHit.collider.TryGetComponent<Unit>(out Unit unit))
            {
                _selectedUnit = unit;
                OnSelectedUnitChanged?.Invoke(_selectedUnit);
                return true;
            }
        }

        return false;
    }
}