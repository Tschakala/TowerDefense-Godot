using System.Numerics;
using Godot;
using Vector2 = Godot.Vector2;

namespace Towerdefense.player;

public partial class CameraController : Camera2D
{
	[Export] private float _sensitivity = 0.25f;
	[Export] private float _zoomSensitivity = 0.025f;
 	private bool _isMoving = false;
	
	public override void _PhysicsProcess(double delta)
	{

	}

	private void MoveCamera(Vector2 delta)
	{
		GlobalPosition += delta;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion && Input.IsActionPressed("camera_move"))
		{
			MoveCamera(-mouseMotion.Relative * _sensitivity);
		}

		if (Input.IsActionJustReleased("camera_zoom"))
		{
			Zoom += Vector2.One * _zoomSensitivity;
			//SetZoom(_currentZoom);
		}
		
		if (Input.IsActionJustReleased("camera_unzoom"))
		{
			Zoom -= Vector2.One * _zoomSensitivity;
			//SetZoom(_currentZoom);
		}
	}
}