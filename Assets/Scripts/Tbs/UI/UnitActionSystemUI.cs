using tbs.actions;
using tbs.units;
using UnityEngine;

namespace tbs.ui
{
    public class UnitActionSystemUI : MonoBehaviour
    {
        [SerializeField] private Transform actionButtonPrefab;
        [SerializeField] private Transform actionButtonContainerTransform;


        private void Start()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;

            CreateUnitActionButtons();
        }


        private void CreateUnitActionButtons()
        {
            foreach (Transform buttonTransform in actionButtonContainerTransform)
            {
                Destroy(buttonTransform.gameObject);
            }

            Unit selectedUnit = UnitActionSystem.Instance.SelectedUnit;

            foreach (BaseAction baseAction in selectedUnit.BaseActionArray)
            {
                Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
                ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
                actionButtonUI.SetBaseAction(baseAction);
            }
        }

        private void UnitActionSystem_OnSelectedUnitChanged(Unit unit)
        {
            CreateUnitActionButtons();
        }


    }
}