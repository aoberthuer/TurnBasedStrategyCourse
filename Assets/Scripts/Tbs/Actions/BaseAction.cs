using tbs.units;
using UnityEngine;

namespace tbs.actions
{
    public abstract class BaseAction : MonoBehaviour
    {
        protected Unit SelectedUnit;
        protected bool IsActive;

        protected virtual void Awake()
        {
            SelectedUnit = GetComponent<Unit>();
        }

    }
}