using grid;
using UnityEngine;

namespace DefaultNamespace
{
    public class Testing : MonoBehaviour
    {
        private GridSystem _gridSystem;

        [SerializeField] private Transform gridDebugObjectTransform;
        
        private void Start()
        {
            _gridSystem = new GridSystem(10, 10, 2f);
            _gridSystem.CreateDebugObjects(gridDebugObjectTransform);
        }

        private void Update()
        {
            Debug.Log(_gridSystem.GetGridPosition(MouseWorld.GetPosition()));
        }
    }
}