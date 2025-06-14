using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManagerpuzzle : MonoBehaviour
{
    public float timeRemaining = 30f;
    public TextMeshProUGUI timerText;
    public PipePathGroup[] colorPaths;

    public GameObject winUI;
    public GameObject loseUI;

    private bool gameEnded = false;

    void Start()
    {
        if (colorPaths == null || colorPaths.Length == 0)
        {
            Debug.LogError("No color paths defined!");
        }

        foreach (var path in colorPaths)
        {
            if (path.startTile == null || path.endTile == null)
            {
                Debug.LogError("A path is missing start or end tile!");
            }
            path.NormalizeColor();
        }

        foreach (var tile in FindObjectsOfType<GridTile>())
        {
            if (tile.GetPipe() == null)
            {
                Debug.LogWarning($"Tile {tile.name} has no pipe assigned!");
            }
            else
            {
                tile.GetPipe().UpdateDirectionByRotation();
            }
            GridManager.Instance?.RegisterTile(tile.gridPosition, tile);
        }

        if (timerText == null) Debug.LogError("Timer Text is not assigned!");
        if (winUI != null) winUI.SetActive(false);
        if (loseUI != null) loseUI.SetActive(false);
    }

    void Update()
    {
        if (gameEnded) return;

        timeRemaining -= Time.deltaTime;
        timeRemaining = Mathf.Max(timeRemaining, 0);
        timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining).ToString();
        timerText.color = timeRemaining < 10f ? Color.red : Color.white;

        if (timeRemaining <= 0)
        {
            CheckGameOver();
        }
        else
        {
            CheckWinCondition();
        }
    }

    void CheckGameOver()
    {
        if (AllPathsConnected())
        {
            Win();
        }
        else
        {
            Lose();
        }
    }

    void CheckWinCondition()
    {
        if (AllPathsConnected())
        {
            Win();
        }
    }

    void Win()
    {
        if (winUI != null) winUI.SetActive(true);
        gameEnded = true;
        Debug.Log("You Win!");
    }

    void Lose()
{
    if (loseUI != null) loseUI.SetActive(true);
    gameEnded = true;
    Debug.Log("You Lose!");

    // Tambahan: pindah scene ke SampleScene setelah kalah
    SceneManager.LoadScene("SampleScene");
}


    bool AllPathsConnected()
    {
        foreach (PipePathGroup path in colorPaths)
        {
            if (path.startTile == null || path.endTile == null)
            {
                Debug.LogWarning("Start or End Tile not set in PipePathGroup.");
                return false;
            }

            if (!IsPathConnected(path.startTile, path.endTile, path.pathColor))
            {
                Debug.Log($"Path from {path.startTile.name} to {path.endTile.name} with color {path.pathColor} is not connected.");
                return false;
            }
        }
        return true;
    }

    bool IsPathConnected(GridTile start, GridTile end, Color pathColor)
    {
        HashSet<GridTile> visited = new HashSet<GridTile>();
        Queue<GridTile> queue = new Queue<GridTile>();
        Dictionary<GridTile, PipeDirection> cameFromDirection = new Dictionary<GridTile, PipeDirection>();

        queue.Enqueue(start);
        visited.Add(start);
        cameFromDirection[start] = PipeDirection.None;

        while (queue.Count > 0)
        {
            GridTile current = queue.Dequeue();
            
            if (current == end)
            {
                Debug.Log($"✓ Path found for color {pathColor}!");
                return true;
            }

            Pipe currentPipe = current.GetPipe();
            if (currentPipe == null) continue;

            // Cek apakah tile ini bisa dilewati oleh warna ini
            if (!CanTileAcceptColor(current, pathColor))
            {
                Debug.Log($"✗ Tile {current.name} cannot accept color {pathColor}");
                continue;
            }

            // Cek semua arah keluar dari pipe saat ini
            List<PipeDirection> exitDirections = GetExitDirections(currentPipe, cameFromDirection[current]);
            
            foreach (PipeDirection exitDir in exitDirections)
            {
                Vector2Int nextPos = current.gridPosition + DirectionHelper.ToVector2Int(exitDir);
                GridTile nextTile = GridManager.Instance?.GetTileAt(nextPos);
                
                if (nextTile == null || visited.Contains(nextTile)) continue;

                Pipe nextPipe = nextTile.GetPipe();
                if (nextPipe == null) continue;

                // Cek apakah pipe tetangga bisa menerima koneksi dari arah ini
                PipeDirection enterDir = DirectionHelper.Opposite(exitDir);
                if (CanPipeAcceptConnection(nextPipe, enterDir))
                {
                    visited.Add(nextTile);
                    queue.Enqueue(nextTile);
                    cameFromDirection[nextTile] = exitDir;
                }
            }
        }

        Debug.Log($"✗ No path found for color {pathColor}");
        return false;
    }

    bool CanTileAcceptColor(GridTile tile, Color pathColor)
    {
        // Jika tile tidak memiliki ExpectedColor (Color.clear/black), bisa menerima warna apapun
        if (tile.ExpectedColor == Color.clear || tile.ExpectedColor == Color.black)
            return true;
        
        // Jika ada ExpectedColor, harus cocok dengan warna path
        return ColorsMatch(tile.ExpectedColor, pathColor);
    }

    bool ColorsMatch(Color color1, Color color2)
    {
        float tolerance = 0.1f;
        return Mathf.Abs(color1.r - color2.r) < tolerance &&
               Mathf.Abs(color1.g - color2.g) < tolerance &&
               Mathf.Abs(color1.b - color2.b) < tolerance;
    }

    List<PipeDirection> GetExitDirections(Pipe pipe, PipeDirection cameFrom)
    {
        List<PipeDirection> exits = new List<PipeDirection>();
        
        // Jika ini adalah tile pertama (start), bisa keluar dari kedua arah
        if (cameFrom == PipeDirection.None)
        {
            exits.Add(pipe.entryDirection);
            exits.Add(pipe.exitDirection);
        }
        else
        {
            // Jika datang dari entryDirection, keluar melalui exitDirection
            if (DirectionHelper.Opposite(cameFrom) == pipe.entryDirection)
            {
                exits.Add(pipe.exitDirection);
            }
            // Jika datang dari exitDirection, keluar melalui entryDirection
            else if (DirectionHelper.Opposite(cameFrom) == pipe.exitDirection)
            {
                exits.Add(pipe.entryDirection);
            }
        }
        
        return exits;
    }

    bool CanPipeAcceptConnection(Pipe pipe, PipeDirection fromDirection)
    {
        return pipe.entryDirection == fromDirection || pipe.exitDirection == fromDirection;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}