using System;
using Godot;
using Towerdefense.combat.enemy;

namespace Towerdefense.systems;

public partial class Spawner : Node2D
{
	[Export] private PackedScene _enemyScene;
	[Export] private float _spawnDelay = 0.03f; //In Seconds
	private Timer _spawnTimer = new Timer();
	private int _amount = 0;
	
	public override void _Ready()
	{
		InitializeSpawnTimer();
	}

	private void InitializeSpawnTimer()
	{
		_spawnTimer.WaitTime = _spawnDelay;
		AddChild(_spawnTimer);
		_spawnTimer.Timeout += OnSpawnTimeout;
		_spawnTimer.Start();
	}

	private void OnSpawnTimeout()
	{
		_amount = GetChildren().Count;
		GD.Print(_amount);
		Enemy enemyInstance = _enemyScene.Instantiate<Enemy>();
		AddChild(enemyInstance);
		enemyInstance.GlobalPosition = GlobalPosition;
		
		Node2D target = GetTree().GetFirstNodeInGroup("TargetEnd") as Node2D;
		if(!IsInstanceValid(target))
		{
			//GD.PrintErr("No Available Targets found");
			return;
		}
		enemyInstance.SetPath(GetNavigationPath(GlobalPosition, target.GlobalPosition));
		
		//GD.Print("[INFO] Enemy successfully spawned");
	}
	
	private Vector2[] GetNavigationPath(Vector2 startPosition, Vector2 targetPosition)
	{
		if (!IsInsideTree())
		{
			return Array.Empty<Vector2>();
		}

		Rid defaultMapRid = GetWorld2D().NavigationMap;
		Vector2[] path = NavigationServer2D.MapGetPath(
			defaultMapRid,
			startPosition,
			targetPosition,
			true,
			2
		);

		foreach (var pos in path)
		{
			GD.Print(pos);
		}
		
		return path;
	}
}