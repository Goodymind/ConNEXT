namespace Flowcharter.utils;

using Flowcharter.flowcharter;

public partial class EnvironmentCamera : Camera2D
{
    bool drag = false;
    Vector2 previousMousePosition = Vector2.Zero;
    Vector2 initialPosition;
    Vector2 targetZoom = Vector2.One;
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton)
        {
            drag = !drag;
            previousMousePosition = eventMouseButton.Position;
            initialPosition = Position;
        }
        else if (@event is InputEventMouseMotion eventMouseMotion && drag)
        {
            Position = (initialPosition + previousMousePosition/Zoom - eventMouseMotion.Position/Zoom);
        }
        if (@event.IsActionReleased("ScrollUp"))
        {
            targetZoom *= 1.10f;
        }
        else if (@event.IsActionReleased("ScrollDown"))
        {
            targetZoom *= 0.90f;
        }
    }
    public override void _Process(double delta)
    {
        Zoom = Zoom.Lerp(targetZoom, 10 * (float)delta);
    }
}