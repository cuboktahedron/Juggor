using Serilog;

namespace Juggor.Game;

public partial class CameraController : Node2D
{
    private static readonly ILogger Logger = Log.ForContext<CameraController>();

    private bool isPanning;

    private Godot.Vector2? prevMousePosition;

    public override void _UnhandledInput(InputEvent ev)
    {
        if (ev is InputEventMouse mouse)
        {
            if (mouse is InputEventMouseButton mouseButton)
            {
                if (mouseButton.IsActionPressed(InputMap.Pan))
                {
                    isPanning = true;
                    prevMousePosition = mouse.Position;
                }
                else if (mouseButton.IsActionReleased(InputMap.Pan))
                {
                    isPanning = false;
                    prevMousePosition = null;
                }
            }

            if (isPanning && prevMousePosition != null)
            {
                DoPan(mouse.Position - prevMousePosition.Value);
                prevMousePosition = mouse.Position;
            }
        }
    }

    private void DoPan(Godot.Vector2 deltaPosition)
    {
        Position -= deltaPosition;
    }
}
