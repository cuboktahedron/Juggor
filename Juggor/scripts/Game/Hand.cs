using System.Diagnostics;
using Juggor.Core.Style;

namespace Juggor.Game;

public partial class Hand : Area2D
{
    private readonly HashSet<Ball> ballsToCatch = new();

    private HandPaths? handPaths;

    private PackedScene ballScene = null!;

    public JuggleMain? JuggleField { get; set; }

    public void ChangeThrowStyle(ThrowStyle throwStyle, bool isSynchronous, bool isRight)
    {
        var tcPoints = new List<ThrowCatchPoint>(throwStyle.ThrowCatchPoints)
            .Select(tcPoint => new ThrowCatchPoint(
                new System.Numerics.Vector2(
                    tcPoint.CatchPt.X * EnvironmentSettings.Settings.HandMovingScale.X,
                    tcPoint.CatchPt.Y * EnvironmentSettings.Settings.HandMovingScale.Y),
                new System.Numerics.Vector2(
                    tcPoint.ThrowPt.X * EnvironmentSettings.Settings.HandMovingScale.X,
                    tcPoint.ThrowPt.Y * EnvironmentSettings.Settings.HandMovingScale.Y)))
            .ToList();

        if (!tcPoints.Any())
        {
            throw new ArgumentException("Throw style must be specified points.");
        }

        handPaths = new HandPaths()
        {
            Name = isRight ? "RightPaths" : "LeftPaths",
            Position = new Vector2(576, 548),
        };

        GetParent().AddChild(handPaths);

        bool isFirst =
             (EnvironmentSettings.Settings.IsMirror && !isRight)
          || (!EnvironmentSettings.Settings.IsMirror && isRight);
        int dx = isFirst ? 0 : 1;
        int sx = isRight ? 1 : -1;
        var handPathVecs = new List<Vector2>();

        for (int i = dx; i < tcPoints.Count; i += 2)
        {
            var catchPoint = tcPoints[i].CatchPt;
            var throwPoint = tcPoints[i].ThrowPt;
            var catchVec = new Vector2(sx * catchPoint.X, catchPoint.Y);
            var throwVec = new Vector2(sx * throwPoint.X, throwPoint.Y);

            handPathVecs.Add(catchVec);
            handPathVecs.Add(throwVec);
        }

        if (tcPoints.Count % 2 == 1)
        {
            dx = (dx + 1) % 2;

            for (int i = dx; i < tcPoints.Count; i += 2)
            {
                var catchPoint = tcPoints[i].CatchPt;
                var throwPoint = tcPoints[i].ThrowPt;
                var catchVec = new Vector2(sx * catchPoint.X, catchPoint.Y);
                var throwVec = new Vector2(sx * throwPoint.X, throwPoint.Y);

                handPathVecs.Add(catchVec);
                handPathVecs.Add(throwVec);
            }
        }

        if (handPathVecs.Any())
        {
            handPathVecs.Add(handPathVecs[0]);
        }

        foreach (var ((a, b), i) in
            handPathVecs.Zip(handPathVecs.Skip(1)).Select((item, i) => (item, i)))
        {
            if (i % 2 == 0)
            {
                var handPath = new HandPath(a, b, HandPathPhase.Catch);
                handPaths.AddPath(handPath, EnvironmentSettings.Settings.TempoRate);
            }
            else
            {
                var handPath = new HandPath(a, b, HandPathPhase.Throw);
                handPaths.AddPath(handPath, EnvironmentSettings.Settings.TempoRate);
            }
        }

        if (isSynchronous)
        {
            return;
        }

        if (!isFirst)
        {
            handPaths.Slide();
        }
    }

    public override void _Ready()
    {
        ballScene = GD.Load<PackedScene>("res://scenes/Game/Ball.tscn");
    }

    public override void _PhysicsProcess(double delta)
    {
        Debug.Assert(handPaths != null, $"{nameof(handPaths)} is required.");
        Position = handPaths.Next();
    }

    public void InitBalls(int ballNum)
    {
        for (int i = 0; i < ballNum; i++)
        {
            var ball = ballScene.Instantiate();
            AddChild(ball);
        }
    }

    public void Throw(Hand to, int height)
    {
        Debug.Assert(JuggleField != null, $"{nameof(JuggleField)} is required.");

        if (height == 0)
        {
            return;
        }

        if (this == to && height == 2)
        {
            return;
        }

        var ball = GetChildren()
            .Where(x => x is Ball)
            .Select(x => (Ball)x)
            .FirstOrDefault();
        if (ball == null)
        {
            GD.PushWarning($"[{Name}] No ball in the hand.");
            return;
        }

        float adjuster = 1f;
        if (height == 1)
        {
            adjuster = 0.5f;
        }

        float fHeight = height;
        var flyingFrame = (long)((fHeight - adjuster) * 60f / EnvironmentSettings.Settings.TempoRate);
        var flyingTime = flyingFrame / 60f;

        RemoveChild(ball);
        JuggleField.AddChild(ball);

        var diffPos = to.CalculateCatchPosition(flyingFrame) - Position;
        var impulse = Vector2.Zero;

        float gravity = EnvironmentSettings.Settings.Gravity;
        impulse.X = (float)(diffPos.X / flyingTime);
        impulse.Y = (float)((diffPos.Y - (0.5 * gravity * flyingTime * flyingTime)) / flyingTime);

        ball.Launch(Position, flyingTime, impulse);
        ball.OnReachedToCatch += to.CatchBall;

        to.AddBallToCatch(ball);
    }

    public Vector2 CalculateCatchPosition(long flyingFrame)
    {
        Debug.Assert(handPaths != null, $"{nameof(handPaths)} is required.");

        var pos = handPaths.PathPositionAfter(flyingFrame);
        GD.Print(flyingFrame, pos);
        return pos;
    }

    private void CatchBall(object? sender, EventArgs e)
    {
        if (sender is not Ball ball)
        {
            return;
        }

        JuggleField?.RemoveChild(ball);
        ball.OnReachedToCatch -= this.CatchBall;
        ball.Reset();
        AddChild(ball);
    }

    private void AddBallToCatch(Ball ball)
    {
        ballsToCatch.Add(ball);
    }
}
