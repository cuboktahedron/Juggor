namespace Juggor.Game;

public partial class HandPath : Path2D
{
    public PathFollow2D PathFollow { get; init; }

    public HandPath(Vector2 from, Vector2 to)
    {
        Curve = new Curve2D();
        Curve.AddPoint(from);
        Curve.AddPoint(to);

        PathFollow = new PathFollow2D
        {
            Loop = false,
        };

        AddChild(PathFollow);
    }

    public float ProgressRatio
    {
        get => PathFollow.ProgressRatio;
        set => PathFollow.ProgressRatio = value;
    }
}
