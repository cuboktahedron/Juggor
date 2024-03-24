using Serilog;

namespace Juggor.Game;

public partial class CameraController : Node2D
{
    private static readonly ILogger Logger = Log.ForContext<CameraController>();

    private bool isPanning;

    private Vector2? prevMousePosition;

    private Camera2D camera = null!;

    private long zoomPermil = 1000;

    [Signal]
    public delegate void OnReverTotHomeViewEventHandler();

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

    public void AdjustCamera(Vector2 yRange)
    {
        Vector2 windowSize = (Vector2)GetViewport().GetWindow().Size;
        long marginTop = 60;
        long viewHeight = (long)windowSize.Y - marginTop;

        long newZoomPermil = (long)(viewHeight / yRange.Y * 1000);
        Logger.Debug($"newZoomPermil={newZoomPermil}");
        Zoom(Vector2.Zero, newZoomPermil);

        Vector2 viewSize = windowSize / ZoomMagnification;
        Position = new Vector2((windowSize.X - viewSize.X) / 2, windowSize.Y - viewSize.Y);
        Logger.Debug($"CameraPosition={Position}");
    }

    private void RevertToHomeView()
    {
        EmitSignal(SignalName.OnReverTotHomeView);
    }

    private void DoPan(Vector2 deltaPosition)
    {
        Position -= deltaPosition / ZoomMagnification;
    }

    private void ZoomIn(Vector2 zoomPosition)
    {
        long newZoomPermil = zoomPermil;
        int digitM1 = (int)Math.Log10(zoomPermil);
        long mod = newZoomPermil % (long)Math.Pow(10, digitM1);
        if (mod > 0)
        {
            newZoomPermil += (long)Math.Pow(10, digitM1) - mod;
        }
        else
        {
            newZoomPermil += (long)Math.Pow(10, digitM1);
        }

        long beforeZSoomPermil = zoomPermil;
        Zoom(zoomPosition, newZoomPermil);
        Logger.Debug($"ZoomIn before={beforeZSoomPermil} after={zoomPermil}");
    }

    private void ZoomOut(Vector2 zoomPosition)
    {
        long newZoomPermil = zoomPermil;
        int digitM1 = (int)Math.Log10(zoomPermil);
        long mod = newZoomPermil % (long)Math.Pow(10, digitM1);
        if (mod > 0)
        {
            newZoomPermil -= mod;
        }
        else
        {
            if (newZoomPermil > (long)Math.Pow(10, digitM1))
            {
                newZoomPermil -= (long)Math.Pow(10, digitM1);
            }
            else
            {
                newZoomPermil -= (long)Math.Pow(10, digitM1 - 1);
            }
        }

        long beforeZSoomPermil = zoomPermil;
        Zoom(zoomPosition, newZoomPermil);
        Logger.Debug($"ZoomOut before={beforeZSoomPermil} after={zoomPermil}");
    }

    private void Zoom(Vector2 zoomPosition, long newZoomPermil)
    {
        Vector2 windowSize = (Vector2)GetViewport().GetWindow().Size;
        Vector2 ratio = zoomPosition / windowSize;
        float beforeZoomMagnification = ZoomMagnification;

        zoomPermil = Math.Max(Math.Min(newZoomPermil, 1000), 1);
        camera.Zoom = new Vector2(1, 1) * ZoomMagnification;

        Vector2 viewSize = windowSize / ZoomMagnification;
        Vector2 nextCameraPosition = Position + (zoomPosition / beforeZoomMagnification);
        nextCameraPosition -= viewSize * ratio;
        Position = nextCameraPosition;
    }
}
