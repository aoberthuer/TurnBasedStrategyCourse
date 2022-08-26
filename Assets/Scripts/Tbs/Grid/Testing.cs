using System.Collections.Generic;
using tbs.grid;
using UnityEngine;

namespace tbs.grid
{
    public class Testing : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
                GridPosition startGridPosition = new GridPosition(0, 0);

                List<GridPosition> gridPositionList =
                    Pathfinder.Instance.FindPath(startGridPosition, mouseGridPosition, out int pathLength);

                for (int i = 0; i < gridPositionList.Count - 1; i++)
                {
                    Debug.DrawLine(
                        LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
                        LevelGrid.Instance.GetWorldPosition(gridPositionList[i + 1]),
                        Color.red,
                        20f
                    );
                }
            }
        }
    }
}