namespace Juggor.Game;

public partial class PauseScreen : Node2D
{
    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed(InputMap.Pause))
        {
            Visible = !Visible;
            GetTree().Paused = !GetTree().Paused;
        }
    }
}
