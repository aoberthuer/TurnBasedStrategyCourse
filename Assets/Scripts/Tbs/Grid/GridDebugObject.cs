using TMPro;
using UnityEngine;

namespace tbs.grid
{
    public class GridDebugObject : MonoBehaviour
    {
        private object _gridObject;

        [SerializeField] private TextMeshPro textMeshPro;

        public virtual void SetGridObject(object gridObject)
        {
            _gridObject = gridObject;
        }

        protected virtual void Update()
        {
            if (_gridObject == null)
                return;

            textMeshPro.text = _gridObject.ToString();
        }
    }
}