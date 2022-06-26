using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] private Unit _selectedUnit;

    [SerializeField] private LayerMask _unitLayerMask;

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
                return true;
            }
        }

        return false;
    }
}