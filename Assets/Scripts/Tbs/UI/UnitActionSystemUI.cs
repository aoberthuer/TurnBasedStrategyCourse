using System;
using System.Collections.Generic;
using tbs.actions;
using tbs.units;
using TMPro;
using UnityEngine;

namespace tbs.ui
{
    public class UnitActionSystemUI : MonoBehaviour
    {
        [SerializeField] private Transform actionButtonPrefab;
        [SerializeField] private Transform actionButtonContainerTransform;

        [SerializeField] private TextMeshProUGUI actionPointsText;

        private List<ActionButtonUI> _actionButtonUiList = new List<ActionButtonUI>();

        private void Start()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
            UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
            UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;

            UpdateActionPoints();
            CreateUnitActionButtons();
            UpdateSelectedVisual();
        }

        private void OnDestroy()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
            UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
        }

        private void CreateUnitActionButtons()
        {
            foreach (Transform buttonTransform in actionButtonContainerTransform)
            {
                Destroy(buttonTransform.gameObject);
            }

            _actionButtonUiList.Clear();
            
            Unit selectedUnit = UnitActionSystem.Instance.SelectedUnit;

            foreach (BaseAction baseAction in selectedUnit.BaseActionArray)
            {
                Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
                ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
                actionButtonUI.SetBaseAction(baseAction);
                _actionButtonUiList.Add(actionButtonUI);
            }
        }
        
        private void UnitActionSystem_OnActionStarted()
        {
            UpdateActionPoints();
        }


        private void UnitActionSystem_OnSelectedUnitChanged(Unit unit)
        {
            CreateUnitActionButtons();
            UpdateSelectedVisual();
            UpdateActionPoints();
        }
        
        private void UnitActionSystem_OnSelectedActionChanged(BaseAction baseAction)
        {
            UpdateSelectedVisual();
        }

        private void UpdateSelectedVisual()
        {
            foreach (ActionButtonUI actionButtonUI in _actionButtonUiList)
            {
                actionButtonUI.UpdateSelectedVisual();
            }
        }

        private void UpdateActionPoints()
        {
            Unit selectedUnit = UnitActionSystem.Instance.SelectedUnit;

            actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();
        }



    }
}