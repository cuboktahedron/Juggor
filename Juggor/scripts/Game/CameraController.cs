using Serilog;

namespace Juggor.Game;

public partial class CameraController : Node2D
{
    private static readonly ILogger Logger = Log.ForContext<CameraController>();

    private bool isPanning;

    private Vector2? prevMousePosition;

    private Camera2D camera = null!;

    private long zoomPermil = 1000;

    private float ZoomMagnification => zoomPermil / 1000f;

    public override void _Ready()
    {
        camera = GetNode<Camera2D>("Camera");
    }

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
                else if (mouseButton.IsActionPressed(InputMap.ZoomIn))
                {
                    ZoomIn(mouse.Position);
                }
                else if (mouseButton.IsActionPressed(InputMap.ZoomOut))
                {
                    ZoomOut(mouse.Position);
                }
            }

            if (isPanning && prevMousePosition != null)
            {
                DoPan(mouse.Position - prevMousePosition.Value);
                prevMousePosition = mouse.Position;
            }
        }
        else if (ev is InputEventKey key)
        {
            if (key.IsActionPressed(InputMap.HomeView))
            {
                RevertToHomeView();
            }
        }
    }

    private void RevertToHomeView()
    {
        zoomPermil = 1000;
        camera.Zoom = new Vector2(1, 1);
        Position = Vector2.Zero;
    }

    private void DoPan(Vector2 deltaPosition)
    {
        Position -= deltaPosition / ZoomMagnification;
    }

    private void ZoomIn(Vector2 zoomPosition)
    {
        Vector2 windowSize = (Vector2)GetViewport().GetWindow().Size;
        Vector2 ratio = zoomPosition / windowSize;
        float beforeZoomMagnification = ZoomMagnification;

        zoomPermil = Math.Min(zoomPermil + 200, 10000);
        camera.Zoom = new Vector2(1, 1) * ZoomMagnification;

        Vector2 viewSize = windowSize / ZoomMagnification;
        Vector2 nextCameraPosition = Position + (zoomPosition / beforeZoomMagnification);
        nextCameraPosition -= viewSize * ratio;
        Position = nextCameraPosition;
    }

    private void ZoomOut(Vector2 zoomPosition)
    {
        Vector2 windowSize = (Vector2)GetViewport().GetWindow().Size;
        Vector2 ratio = zoomPosition / windowSize;
        float beforeZoomMagnification = zoomPermil / 1000f;

        zoomPermil = Math.Max(zoomPermil - 200, 200);
        camera.Zoom = new Vector2(1, 1) * ZoomMagnification;

        Vector2 viewSize = windowSize / ZoomMagnification;
        Vector2 nextCameraPosition = Position + (zoomPosition / beforeZoomMagnification);
        nextCameraPosition -= viewSize * ratio;
        Position = nextCameraPosition;
    }
}
