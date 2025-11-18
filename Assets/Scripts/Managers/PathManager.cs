using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Managers
{
    public class PathManager : MonoBehaviour
    {
        [SerializeField] private Tilemap pathTile;

        private void Awake()
        {
            CreatePath();
        }

        private void CreatePath()
        {
            List<Vector3Int> pathCells = new List<Vector3Int>();
            HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

            //Find starter tile
            foreach (var pos in pathTile.cellBounds.allPositionsWithin)
            {
                if (pathTile.HasTile(pos))
                {
                    pathCells.Add(pos);
                    break;
                }
            }

            while (pathCells.Count > 0)
            {
                var current = pathCells[^1];
                visited.Add(current);
                
                //Fina way from the closest tile
                Vector3Int[] closestTile =
                {
                    new Vector3Int(1, 0, 0),
                    new Vector3Int(-1, 0, 0),
                    new Vector3Int(0, 1, 0),
                    new Vector3Int(0, -1, 0)
                };

                var next = Vector3Int.zero;
                bool found = false;

                foreach (var dir in closestTile)
                {
                    Vector3Int neighbor = current + dir;
                    if (pathTile.HasTile(neighbor) && !visited.Contains(neighbor))
                    {
                        next = neighbor;
                        found = true;
                        break;
                    }
                }

                if (found)
                    pathCells.Add(next);
                else
                    break;
            }

            List<Vector3> wayPoints = new();
            foreach (var c in pathCells)
                wayPoints.Add(pathTile.GetCellCenterWorld(c));

            EventBus.Publish(new PathConstructedEvent(wayPoints));
            Debug.Log($"[PathManager] Published PathConstructedEvent with {wayPoints.Count} points!");
        }
    }
}
