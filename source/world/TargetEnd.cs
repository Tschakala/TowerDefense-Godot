using Godot;
using Enemy = Towerdefense.source.combat.enemy.Enemy;

namespace Towerdefense.source.world;

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