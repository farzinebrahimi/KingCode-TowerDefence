using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Managers
{
    public class PathManager : MonoBehaviour
    {
        [Header("Tilemap Settings")]
        [SerializeField] private Tilemap pathTilemap;
         private List<Vector3> tilePositions = new();
        [SerializeField] private bool debugMode = true;

        private void Start()
        {
            GetAllTilePositions(); 
            
        }

        void GetAllTilePositions()
        {
            tilePositions.Clear();
            
            List<Vector3Int> allCells = new List<Vector3Int>();
            foreach (var pos in pathTilemap.cellBounds.allPositionsWithin) 
            {
                if (pathTilemap.HasTile(pos)) 
                    allCells.Add(pos);
            }

            if (allCells.Count == 0) return;
            
            Vector3Int startCell = allCells[0];
            foreach (var cell in allCells)
            {
                if (cell.x < startCell.x )
                {
                    startCell = cell;
                }
                else if (startCell.x == startCell.x && startCell.y > startCell.y)
                {
                    startCell = cell;
                }
            }

            List<Vector3Int> sortedCells = new List<Vector3Int>();
            HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
            
            Vector3Int current = startCell;
            sortedCells.Add(current);
            visited.Add(current);

            while (sortedCells.Count < allCells.Count)
            {
                Vector3Int closest = Vector3Int.zero;
                float minDistance = float.MaxValue;

                foreach (var cell in allCells)
                {
                    if (visited.Contains(cell)) continue;

                    float distance = Vector3Int.Distance(current, cell);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closest = cell;
                    }
                }

                if (closest != Vector3Int.zero)
                {
                    sortedCells.Add(closest);
                    visited.Add(closest);
                    current = closest;
                }
                else
                {
                    break;
                }
            }

            foreach (var cellPos in sortedCells)
            {
                Vector3 worldPos = pathTilemap.GetCellCenterWorld(cellPos); 
                tilePositions.Add(worldPos);
            }
            EventBus.Publish(new PathConstructedEvent(tilePositions));

        }

        private void OnDrawGizmos()
        {
            if (tilePositions == null || tilePositions.Count == 0) return;

            Gizmos.color = Color.green;
            for (int i = 0; i < tilePositions.Count; i++)
            {
                Gizmos.DrawSphere(tilePositions[i], 0.3f);
                
                if (i > 0)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(tilePositions[i - 1], tilePositions[i]);
                    Gizmos.color = Color.green;
                }
            }
        }
    }
}
