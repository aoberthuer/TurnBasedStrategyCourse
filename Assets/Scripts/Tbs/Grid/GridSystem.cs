using UnityEngine;

namespace tbs.grid
{
    public class GridSystem
    {
        private readonly int _width;
        public int Width => _width;
        
        private readonly int _height;
        public int Height => _height;
        
        private readonly float _cellSize;

        private readonly GridObject[,] _gridObjects;

        public GridSystem(int width, int height, float cellSize)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;

            _gridObjects = new GridObject[_width, _height];
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    _gridObjects[x, z] = new GridObject(this, gridPosition);
                }
            }
        }

        public Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            return new Vector3(gridPosition.x, 0, gridPosition.z) * _cellSize;
        }

        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            return new GridPosition(
                Mathf.RoundToInt(worldPosition.x / _cellSize),
                Mathf.RoundToInt(worldPosition.z / _cellSize)
            );
        }

        public GridObject GetGridObject(GridPosition gridPosition)
        {
            return _gridObjects[gridPosition.x, gridPosition.z];
        }
        
        public bool IsValidGridPosition(GridPosition gridPosition)
        {
            return  gridPosition.x >= 0 && 
                    gridPosition.z >= 0 && 
                    gridPosition.x < _width && 
                    gridPosition.z < _height;
        }



        public void CreateDebugObjects(Transform debugPrefab)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    Transform transform =
                        GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                    GridDebugObject gdo = transform.GetComponent<GridDebugObject>();
                    gdo.SetGridObject(GetGridObject(gridPosition));
                }
            }
        }
    }
}