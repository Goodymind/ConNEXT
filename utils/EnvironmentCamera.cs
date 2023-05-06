using Godot;
using Flowcharter.flowcharter;
namespace Flowcharter.utils;

public partial class EnvironmentCamera : Camera2D
{
    bool drag = false;
    Vector2 previousMousePosition = Vector2.Zero;
    Vector2 initialPosition;
    Vector2 targetZoom = Vector2.One;
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionReleased("ScrollUp"))
        {
            targetZoom *= 1.10f;
        }
        else if (@event.IsActionReleased("ScrollDown"))
        {
            targetZoom *= 0.90f;
        }
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (eventMouseButton.ButtonIndex != MouseButton.Left)
                return;
            drag = eventMouseButton.Pressed;
            previousMousePosition = eventMouseButton.Position;
            initialPosition = Position;
        }
        else if (@event is InputEventMouseMotion eventMouseMotion && drag)
        {
            Position = (initialPosition + previousMousePosition/Zoom - eventMouseMotion.Position/Zoom);
        }
    }
    public override void _Process(double delta)
    {
        Zoom = Zoom.Lerp(targetZoom, 10 * (float)delta);
    }
}