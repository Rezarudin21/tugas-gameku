using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject pipePrefab;
    public Transform spawnPoint;
    public int maxPipes = 6;

    private int pipeCount = 0;
    private bool canSpawn = true;

    private void OnMouseDown()
    {
        if (!canSpawn || pipeCount >= maxPipes) return;
        if (pipePrefab == null || spawnPoint == null) return;

        GameObject newPipe = Instantiate(pipePrefab, spawnPoint.position, Quaternion.identity);

        Drag2D drag = newPipe.GetComponent<Drag2D>();
        if (drag != null)
        {
            drag.OnPipePlaced += HandlePipePlaced;
            drag.StartDraggingFromSpawner();
        }

        pipeCount++;
        canSpawn = false;
    }

    private void HandlePipePlaced()
    {
        canSpawn = true;
    }
}
