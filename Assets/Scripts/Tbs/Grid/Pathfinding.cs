using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace tbs.grid
{
    public class Pathfinding : MonoBehaviour
    {
        public static Pathfinding Instance { get; private set; }

        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        [FormerlySerializedAs("gridDebugObjectPrefab")] [SerializeField] private Transform _gridDebugObjectPrefab;
        
        [SerializeField] private LayerMask _obstaclesLayerMask;
        [SerializeField] private bool _displayDebugRaytraces;
        
        private GridSystem<PathNode> _gridSystem;
        private int _width;
        private int _height;
        private float _cellSize;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There's more than one Pathfinding! " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            Instance = this;

        }

        public void Setup(int width, int height, float cellSize)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;

            _gridSystem = new GridSystem<PathNode>(width, height, cellSize,
                (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
            _gridSystem.CreateDebugObjects(transform, _gridDebugObjectPrefab);

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    
                    GridPosition gridPosition = new GridPosition(x, z);
                    Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                    float raycastOffsetDistance = 5f;

                    if (_displayDebugRaytraces)
                    {
                        Debug.DrawLine(
                            worldPosition + Vector3.down * raycastOffsetDistance,
                            worldPosition + Vector3.up * raycastOffsetDistance,
                            Color.red,
                            60f
                        );    
                    }
                    
                    if (Physics.Raycast(
                            worldPosition + Vector3.down * raycastOffsetDistance,
                            Vector3.up,
                            raycastOffsetDistance * 2,
                            _obstaclesLayerMask))
                    {
                        GetNode(x, z).SetIsWalkable(false);
                    }
                    
                }
            }
        }

        
        

        public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
        {
            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closedList = new List<PathNode>();

            PathNode startNode = _gridSystem.GetGridObject(startGridPosition);
            PathNode endNode = _gridSystem.GetGridObject(endGridPosition);
            openList.Add(startNode);

            ClearPathNodes();

            // Set and calculate G-, H- and F-cost on start node,
            // start node is only node in open list so far.
            startNode.SetGCost(0);
            startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
            startNode.CalculateFCost();

            // now start form start node
            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostPathNode(openList);

                if (currentNode == endNode)
                {
                    // Reached final node
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                // Search all neighbours of current node
                foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    // This neighbour has already been searched...
                    if (closedList.Contains(neighbourNode))
                    {
                        continue;
                    }
                    
                    if (!neighbourNode.IsWalkable())
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }

                    int tentativeGCost =
                        currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(),
                            neighbourNode.GetGridPosition());

                    // Essentially this means, we have found a better path to go to the neighbour node from the current node
                    if (tentativeGCost < neighbourNode.GetGCost())
                    {
                        neighbourNode.SetCameFromPathNode(currentNode);
                        neighbourNode.SetGCost(tentativeGCost);
                        neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                        neighbourNode.CalculateFCost();

                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }

            // No path found
            return null;
        }

        private void ClearPathNodes()
        {
            for (int x = 0; x < _gridSystem.Width; x++)
            {
                for (int z = 0; z < _gridSystem.Height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    PathNode pathNode = _gridSystem.GetGridObject(gridPosition);

                    pathNode.SetGCost(int.MaxValue);
                    pathNode.SetHCost(0);
                    pathNode.CalculateFCost();
                    pathNode.ResetCameFromPathNode();
                }
            }
        }

        private int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
        {
            GridPosition gridPositionDistance = gridPositionA - gridPositionB;

            int xDistance = Mathf.Abs(gridPositionDistance.x);
            int zDistance = Mathf.Abs(gridPositionDistance.z);
            int remaining = Mathf.Abs(xDistance - zDistance);

            // Move diagonally for as many x as we have z. Move straight for the rest.
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostPathNode = pathNodeList[0];
            for (int i = 0; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
                {
                    lowestFCostPathNode = pathNodeList[i];
                }
            }

            return lowestFCostPathNode;
        }

        private PathNode GetNode(int x, int z)
        {
            return _gridSystem.GetGridObject(new GridPosition(x, z));
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            List<PathNode> neighbourList = new List<PathNode>();

            GridPosition gridPosition = currentNode.GetGridPosition();

            if (gridPosition.x - 1 >= 0)
            {
                // Left
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));
                if (gridPosition.z - 1 >= 0)
                {
                    // Left Down
                    neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
                }

                if (gridPosition.z + 1 < _gridSystem.Height)
                {
                    // Left Up
                    neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
                }
            }

            if (gridPosition.x + 1 < _gridSystem.Width)
            {
                // Right
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));
                if (gridPosition.z - 1 >= 0)
                {
                    // Right Down
                    neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
                }

                if (gridPosition.z + 1 < _gridSystem.Height)
                {
                    // Right Up
                    neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
                }
            }

            if (gridPosition.z - 1 >= 0)
            {
                // Down
                neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
            }

            if (gridPosition.z + 1 < _gridSystem.Height)
            {
                // Up
                neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
            }

            return neighbourList;
        }

        private List<GridPosition> CalculatePath(PathNode endNode)
        {
            List<PathNode> pathNodeList = new List<PathNode>();
            pathNodeList.Add(endNode);
            PathNode currentNode = endNode;
            while (currentNode.GetCameFromPathNode() != null)
            {
                pathNodeList.Add(currentNode.GetCameFromPathNode());
                currentNode = currentNode.GetCameFromPathNode();
            }

            pathNodeList.Reverse();

            List<GridPosition> gridPositionList = new List<GridPosition>();
            foreach (PathNode pathNode in pathNodeList)
            {
                gridPositionList.Add(pathNode.GetGridPosition());
            }

            return gridPositionList;
        }
        
    }
}