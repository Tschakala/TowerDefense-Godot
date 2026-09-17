using Godot;

namespace Towerdefense.scripts;

public partial class TargetEnd : Node2D
{
	private void OnBodyEntered(Node2D body)
	{
		if (body is Enemy enemy)
		{
			enemy.QueueFree();
		}
	}
}