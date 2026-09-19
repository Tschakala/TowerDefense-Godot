using System;
using Godot;
using Vector2 = Godot.Vector2;
using Towerdefense.source.core;

namespace Towerdefense.source.combat.player;

public partial class CameraController : Camera2D
{
	[Export] private Resource _defaultCursor;
	[Export] private Resource _grabCursor;
	
	private static readonly Config Conf = new Config();
	
	private float _sensitivity = Conf.Sensitivity;
	private float _zoomSensitivity = Conf.ZoomSensitivity;
 	private bool _isMoving;

	private void MoveCamera(Vector2 delta)
	{
		GlobalPosition += delta;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			if (Input.IsActionPressed("camera_move"))
			{
				MoveCamera(-mouseMotion.Relative / GetZoom().X);
				Input.SetCustomMouseCursor(_grabCursor);
			}
			else
			{
				Input.SetCustomMouseCursor(_defaultCursor);
			}
		}

		if (Input.IsActionJustReleased("camera_zoom"))
		{
			Zoom += Vector2.One * _zoomSensitivity;
			Zoom = new Vector2(Math.Min(2,  Zoom.X), Math.Min(2, Zoom.Y));
			//SetZoom(_currentZoom);
		}
		
		if (Input.IsActionJustReleased("camera_unzoom"))
		{
			Zoom -= Vector2.One * _zoomSensitivity;
			Zoom = new Vector2(Math.Max(0.1f,  Zoom.X), Math.Max(0.1f, Zoom.Y));
			//SetZoom(_currentZoom);
		}
	}
}