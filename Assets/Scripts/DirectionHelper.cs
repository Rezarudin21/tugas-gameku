using UnityEngine;

public static class DirectionHelper
{
    public static Vector2Int ToVector2Int(PipeDirection direction)
{
    return direction switch
    {
        PipeDirection.Up => new Vector2Int(0, 1),
        PipeDirection.Down => new Vector2Int(0, -1),
        PipeDirection.Left => new Vector2Int(-1, 0),
        PipeDirection.Right => new Vector2Int(1, 0),
        _ => Vector2Int.zero,
    };
}

    public static PipeDirection Opposite(PipeDirection dir)
{
    return dir switch
    {
        PipeDirection.Up => PipeDirection.Down,
        PipeDirection.Down => PipeDirection.Up,
        PipeDirection.Left => PipeDirection.Right,
        PipeDirection.Right => PipeDirection.Left,
        _ => dir,
    };
}

}
