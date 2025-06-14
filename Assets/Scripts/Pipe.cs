using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Color pipeColor = Color.white;

    public PipeDirection entryDirection;
    public PipeDirection exitDirection;
    public bool isCorner = false;

    [Header("Background")]
    public GameObject backgroundObject;

    private bool isPlaced = false;

    private void Start()
    {
        UpdateDirectionByRotation();
    }

   public void UpdateDirectionByRotation()
{
    switch (Mathf.RoundToInt(transform.eulerAngles.z))
    {
        case 0:
            entryDirection = PipeDirection.Left;
            exitDirection = PipeDirection.Right;
            break;
        case 90:
            entryDirection = PipeDirection.Up;
            exitDirection = PipeDirection.Right;
            break;
        case 180:
            entryDirection = PipeDirection.Right;
            exitDirection = PipeDirection.Left;
            break;
        case 270:
            entryDirection = PipeDirection.Down;
            exitDirection = PipeDirection.Left;
            break;
        default:
            Debug.LogWarning("Pipe rotation unrecognized.");
            break;
    }
}



    public void RotatePipe()
    {
        transform.Rotate(0f, 0f, 90f);
        UpdateDirectionByRotation();
    }

    public PipeDirection GetOtherDirection(PipeDirection from)
    {
        if (from == entryDirection) return exitDirection;
        if (from == exitDirection) return entryDirection;
        return PipeDirection.None;
    }

    public void SetPlaced(bool placed)
    {
        isPlaced = placed;
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }

    public void ShowBackground()
    {
        if (backgroundObject != null)
            backgroundObject.SetActive(true);
    }

    public void HideBackground()
    {
        if (backgroundObject != null)
            backgroundObject.SetActive(false);
    }
}
