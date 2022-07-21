using TMPro;
using UnityEngine;

namespace tbs.grid
{
    public class GridDebugObject : MonoBehaviour
    {
        private GridObject _gridObject;

        [SerializeField] private TextMeshPro textMeshPro;

        public void SetGridObject(GridObject gridObject)
        {
            _gridObject = gridObject;
        }

        private void Update()
        {
            if (_gridObject == null)
                return;

            textMeshPro.text = _gridObject.ToString();
        }
    }
}