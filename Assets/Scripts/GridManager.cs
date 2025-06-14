using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    private Dictionary<Vector2Int, GridTile> gridTiles = new Dictionary<Vector2Int, GridTile>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterTile(Vector2Int position, GridTile tile)
    {
        gridTiles[position] = tile;
    }

    public List<GridTile> GetAllTiles()
    {
        return new List<GridTile>(gridTiles.Values);
    }

    public GridTile GetTileAt(Vector2Int pos)
    {
        return gridTiles.ContainsKey(pos) ? gridTiles[pos] : null;
    }
}
