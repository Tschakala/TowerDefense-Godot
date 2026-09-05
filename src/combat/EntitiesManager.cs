using Godot;
using Godot.Collections;
using Towerdefense.combat.enemy;

namespace Towerdefense.combat;

public partial class EntitiesManager : Node2D
{
	[Export] private Node2D _enemyTarget;
	
	private Timer _updateTimer = new Timer();
	
	public override void _Ready()
	{
		IntializeTimer();
	}

	private void IntializeTimer()
	{
		_updateTimer.Autostart = true;
		_updateTimer.WaitTime = 1;
		_updateTimer.Timeout += OnSpawnSpawnTimerOut;
		AddChild(_updateTimer);
		_updateTimer.Start();
	}
	
	private void OnSpawnSpawnTimerOut()
	{
		Array<Node> enemiesNodes = GetTree().GetNodesInGroup("Enemies");
		
		GD.Print("enemies nodes count: " + enemiesNodes.Count);

		foreach (Node enemyNode in enemiesNodes)
		{
			BasicEnemy enemy = enemyNode as BasicEnemy;
			if(!IsInstanceValid(enemy))
				continue;
			
			GD.Print("Setting Enemy's Target...");
			
			enemy.SetTargetPosition(_enemyTarget.GlobalPosition);
		}
	}
}