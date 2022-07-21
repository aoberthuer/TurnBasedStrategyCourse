using UnityEngine;

namespace tbs.units
{
    public class UnitSelectedVisual : MonoBehaviour
    {
        [SerializeField] private Unit _unit;

        private MeshRenderer _meshRenderer;
        
        
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged += HandleSelectionChange;
            UpdateVisual(UnitActionSystem.Instance.SelectedUnit);
        }

        private void OnDestroy()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged -= HandleSelectionChange;
        }

        private void HandleSelectionChange(Unit unit)
        {
            UpdateVisual(unit);
        }

        private void UpdateVisual(Unit unit)
        {
            if (unit == _unit)
            {
                _meshRenderer.enabled = true;
            }
            else
            {
                _meshRenderer.enabled = false;
            }
        }
    }
}