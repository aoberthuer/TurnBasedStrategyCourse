using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Testing : MonoBehaviour
    {
        private GridSystem _gridSystem;

        private void Start()
        {
            _gridSystem = new GridSystem(10, 10, 2f);
        }

        private void Update()
        {
            Debug.Log(_gridSystem.GetGridPosition(MouseWorld.GetPosition()));
        }
    }
}