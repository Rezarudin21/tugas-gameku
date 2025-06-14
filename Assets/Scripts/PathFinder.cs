using System.Collections.Generic;
using UnityEngine;

public static class PathFinder
{
    public static bool IsPathConnected(GridTile start, GridTile end, Color pathColor)
    {
        HashSet<GridTile> visited = new HashSet<GridTile>();
        Debug.Log($"[PathFinder] Starting DFS from {start.name} to {end.name} with color {pathColor}");
        return DFS(start, end, pathColor, visited, null);
    }

    private static bool DFS(GridTile current, GridTile target, Color pathColor, HashSet<GridTile> visited, PipeDirection? cameFrom)
    {
        if (current == null)
        {
            Debug.LogWarning("[DFS] Current tile is null.");
            return false;
        }

        if (visited.Contains(current))
        {
            Debug.Log($"[DFS] Already visited {current.name}");
            return false;
        }

        Pipe pipe = current.GetPipe();
        if (pipe == null)
        {
            Debug.LogWarning($"[DFS] No pipe on {current.name}");
            return false;
        }

        if (current.ExpectedColor != Color.black && current.ExpectedColor != pathColor)
        {
            Debug.LogWarning($"[DFS] Color mismatch at {current.name}: expected {current.ExpectedColor}, got {pathColor}");
            return false;
        }

        visited.Add(current);
        Debug.Log($"[DFS] Visiting {current.name}");

        if (current == target)
        {
            Debug.Log($"[DFS] Reached target {target.name}!");
            return true;
        }

        List<PipeDirection> directions = new List<PipeDirection>();

        if (cameFrom == null || pipe.entryDirection == cameFrom)
            directions.Add(pipe.exitDirection);
        if (cameFrom == null || pipe.exitDirection == cameFrom)
            directions.Add(pipe.entryDirection);

        foreach (PipeDirection dir in directions)
        {
            Vector2Int offset = DirectionToOffset(dir);
            Vector2Int nextPos = current.gridPosition + offset;

            GridTile nextTile = GridManager.Instance.GetTileAt(nextPos);
            if (nextTile == null)
            {
                Debug.Log($"[DFS] No tile at {nextPos} from {current.name} going {dir}");
                continue;
            }

            Pipe nextPipe = nextTile.GetPipe();
            if (nextPipe == null)
            {
                Debug.Log($"[DFS] No pipe on {nextTile.name}");
                continue;
            }

            PipeDirection opposite = GetOppositeDirection(dir);

            if (nextPipe.entryDirection == opposite || nextPipe.exitDirection == opposite)
            {
                if (DFS(nextTile, target, pathColor, visited, opposite))
                    return true;
            }
            else
            {
                Debug.Log($"[DFS] Pipe at {nextTile.name} does not connect properly (expected {opposite})");
            }
        }

        return false;
    }

    private static Vector2Int DirectionToOffset(PipeDirection dir)
    {
        return dir switch
        {
            PipeDirection.Up => Vector2Int.up,
            PipeDirection.Down => Vector2Int.down,
            PipeDirection.Left => Vector2Int.left,
            PipeDirection.Right => Vector2Int.right,
            _ => Vector2Int.zero
        };
    }

    private static PipeDirection GetOppositeDirection(PipeDirection dir)
    {
        return dir switch
        {
            PipeDirection.Up => PipeDirection.Down,
            PipeDirection.Down => PipeDirection.Up,
            PipeDirection.Left => PipeDirection.Right,
            PipeDirection.Right => PipeDirection.Left,
            _ => PipeDirection.None
        };
    }
}
