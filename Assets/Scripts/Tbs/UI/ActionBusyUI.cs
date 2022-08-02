using System;
using tbs.units;
using UnityEngine;

namespace tbs.ui
{
    public class ActionBusyUI : MonoBehaviour
    {
        private void Start()
        {
            UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;

            Hide();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void UnitActionSystem_OnBusyChanged(bool isBusy)
        {
            if (isBusy)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }
}