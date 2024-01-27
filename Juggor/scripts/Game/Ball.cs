namespace Juggor.Game;

public partial class Ball : RigidBody2D
{
    public event EventHandler<EventArgs>? OnReachedToCatch;

    public double RestTimeToCatch { get; set; } = -1;

    public bool IsFlying { get; private set; }

    public bool IsCatchable => RestTimeToCatch <= 0f;

    public override void _Process(double delta)
    {
        RestTimeToCatch -= delta;
        if (RestTimeToCatch <= 0f)
        {
            OnReachedToCatch?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Launch(Vector2 position, double timeToFly, Vector2 impulse)
    {
        Position = position;
        LinearVelocity = Vector2.Zero;
        RestTimeToCatch = timeToFly;
        Freeze = false;
        ApplyImpulse(impulse);

        IsFlying = true;
    }

    public void Reset()
    {
        RestTimeToCatch = -1;
        IsFlying = false;
        LinearVelocity = Vector2.Zero;
        Position = Vector2.Zero;
        Freeze = true;
    }
}
