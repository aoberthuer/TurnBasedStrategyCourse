using UnityEngine;

namespace tbs.grid
{
    public class Pathfinding : MonoBehaviour
    {
        [SerializeField] private Transform gridDebugObjectPrefab;

        private int _width;
        private int _height;
        private float _cellSize;
        private GridSystem<PathNode> _gridSystem;

        private void Awake()
        {
            _gridSystem = new GridSystem<PathNode>(10, 10, 2f,
                (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));

            _gridSystem.CreateDebugObjects(transform, gridDebugObjectPrefab);
        }
    }
}