using Godot;
using Towerdefense.combat.enemy;

namespace Towerdefense.systems;

public partial class TargetEnd : Node2D
{
	private void OnBodyEntered(Node2D body)
	{
		if (body is Enemy enemy)
		{
			enemy.QueueFree();

			//GD.Print("Enemy reached target!");
		}
	}
}