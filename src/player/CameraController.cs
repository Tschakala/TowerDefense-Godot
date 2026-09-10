using Godot;

namespace Towerdefense.player;

public partial class CameraController : Camera2D
{
	[Export] private float _sensitivity = 0.25f;
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
			Input.MouseMode = Input.MouseModeEnum.Captured;
			MoveCamera(-mouseMotion.Relative * _sensitivity);
		}
		else
		{
			Input.MouseMode = Input.MouseModeEnum.Confined;
		}
	}
}