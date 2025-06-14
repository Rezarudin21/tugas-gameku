using UnityEngine;

public class Drag2D : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool isPlaced = false;
    private Vector3 mouseDownPos;
    private float clickThreshold = 0.1f;

    public System.Action OnPipePlaced;

    private static Drag2D activePipe;

    private Pipe pipeScript;
    private GridTile currentTile;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        pipeScript = GetComponent<Pipe>();
        if (pipeScript != null)
            pipeScript.SetPlaced(false); // Mulai dengan background mati
    }

    void OnMouseDown()
    {
        mouseDownPos = Input.mousePosition;
        offset = transform.position - GetMouseWorldPos();
        isDragging = false;
        activePipe = this;

        if (pipeScript != null)
            pipeScript.SetPlaced(false); // Sembunyikan saat drag dimulai
    }

    void OnMouseDrag()
    {
        if (Vector3.Distance(Input.mousePosition, mouseDownPos) > clickThreshold)
        {
            isDragging = true;
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    void OnMouseUp()
    {
        if (isDragging)
        {
            Transform nearestTile = GetOverlappingTile();
            if (nearestTile != null)
            {
                transform.position = nearestTile.position;
                isPlaced = true;

                currentTile = nearestTile.GetComponent<GridTile>();
                if (currentTile != null && pipeScript != null)
                    currentTile.SetPipe(pipeScript);

                if (pipeScript != null)
                    pipeScript.SetPlaced(true); // Tampilkan background

                OnPipePlaced?.Invoke();
            }
            else
            {
                isPlaced = false;

                if (pipeScript != null)
                    pipeScript.SetPlaced(false); // Tetap sembunyikan background
            }
        }
    }

    void Update()
    {
        if (activePipe == this && isPlaced)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.Rotate(0f, 0f, 90f);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1f;
                transform.localScale = scale;
            }

            if (Input.GetMouseButtonDown(1))
            {
                transform.position = originalPosition;
                transform.rotation = originalRotation;
                isPlaced = false;

                if (pipeScript != null)
                    pipeScript.SetPlaced(false); // Sembunyikan saat undo

                if (currentTile != null)
                {
                    currentTile.SetPipe(null);
                    currentTile = null;
                }
            }
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    Transform GetOverlappingTile()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("GridTile"))
            {
                return hit.transform;
            }
        }
        return null;
    }

    public void StartDraggingFromSpawner()
    {
        offset = transform.position - GetMouseWorldPos();
        isDragging = true;
        activePipe = this;

        if (pipeScript != null)
            pipeScript.SetPlaced(false); // Background tetap mati saat mulai drag dari spawner
    }
}