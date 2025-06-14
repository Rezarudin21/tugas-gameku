using UnityEngine;

[System.Serializable]
public class PipePathGroup
{
    public GridTile startTile;
    public GridTile endTile;
    public Color pathColor = Color.white;

    // Tambahkan ini agar bisa dipanggil di GameManager
    public void NormalizeColor()
    {
        pathColor.a = 1f; // memastikan alpha = 1
    }
}
