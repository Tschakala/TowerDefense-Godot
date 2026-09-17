using Godot;
using Vector2 = Godot.Vector2;

namespace Towerdefense.scripts;

public partial class CameraController : Camera2D
{
	private static readonly Config Conf = new Config();
	
	private float _sensitivity = Conf.Sensitivity;
	private float _zoomSensitivity = Conf.ZoomSensitivity;
 	private bool _isMoving;
	
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