namespace Juggor.Game;

public partial class HandPath : Path2D
{
    public HandPath(Vector2 from, Vector2 to, HandPathPhase phase)
    {
        Curve = new Curve2D();
        if (phase == HandPathPhase.Catch)
        {
            var yDiff = EnvironmentSettings.Settings.HandMovingScale.Y * 4;
            var mid = new Vector2((from.X + to.X) / 2f, to.Y + yDiff);
            Curve.AddPoint(from, null, new Vector2(0, yDiff));
            Curve.AddPoint(mid);
            Curve.AddPoint(to, new Vector2(0, yDiff));
        }
        else
        {
            var yDiff = -EnvironmentSettings.Settings.HandMovingScale.Y * 2;
            var mid = new Vector2((from.X + to.X) / 2f, to.Y + yDiff);
            Curve.AddPoint(from, null, new Vector2(0, yDiff));
            Curve.AddPoint(mid);
            Curve.AddPoint(to, new Vector2(0, yDiff));
        }

        PathFollow = new PathFollow2D
        {
            Loop = false,
        };

        AddChild(PathFollow);
    }

    public PathFollow2D PathFollow { get; init; }

    public float ProgressRatio
    {
        get => PathFollow.ProgressRatio;
        set => PathFollow.ProgressRatio = value;
    }
}
