using UnityEngine;

public class GridTile : MonoBehaviour
{
    public Vector2Int gridPosition;
    private Pipe currentPipe;

    public Color ExpectedColor = Color.clear; // Warna default dianggap netral
    void Start()
    {
        if (currentPipe != null)
        {
            currentPipe.UpdateDirectionByRotation();
        }

        GridManager.Instance?.RegisterTile(gridPosition, this);
    }



    public void SetPipe(Pipe pipe)
    {
        currentPipe = pipe;
    }

    public Pipe GetPipe()
    {
        return currentPipe;
    }
}
