﻿using System;
using tbs.units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace tbs.ui
{
    public class UnitWorldUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI actionPointsText;
        [SerializeField] private Unit unit;
        [SerializeField] private Image healthBarImage;
        [SerializeField] private HealthSystem healthSystem;

        private void Start()
        {
            Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
            healthSystem.OnDamaged += HealthSystem_OnDamaged;

            UpdateActionPointsText();
            UpdateHealthBar();
        }

        private void UpdateActionPointsText()
        {
            actionPointsText.text = unit.GetActionPoints().ToString();
        }

        private void Unit_OnAnyActionPointsChanged()
        {
            UpdateActionPointsText();
        }

        private void UpdateHealthBar()
        {
            healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
        }

        private void HealthSystem_OnDamaged()
        {
            UpdateHealthBar();
        }

        
    }
}