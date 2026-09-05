using Godot;

namespace Towerdefense.core;

public partial class EnemySpawner : Node
{
	[Export] private PackedScene _enemyScene;
	[Export] private Node2D _rootNode;
	[Export] private Node2D _enemySpawnCross;

	private Timer _spawnTimer = new Timer();

	public override void _Ready()
	{
		_spawnTimer.Autostart = true;
		_spawnTimer.WaitTime = 1;
		_spawnTimer.Timeout += OnSpawnSpawnTimerOut;
		AddChild(_spawnTimer);
		_spawnTimer.Start();
	}

	private void OnSpawnSpawnTimerOut()
	{
		Node2D enemyInstance = _enemyScene.Instantiate<Node2D>();
		_rootNode.AddChild(enemyInstance);
		enemyInstance.GlobalPosition = _enemySpawnCross.GlobalPosition;
	}
}