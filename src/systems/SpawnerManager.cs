using System.Collections.Generic;
using Godot;
using Towerdefense.combat.enemy;

namespace Towerdefense.systems;

public partial class SpawnerManager : Node2D
{
	[Export] private PackedScene _enemyScene;
	[Export] private float _spawnDelay = 0.05f; //In Seconds
	[Export] private int _maxEnemies = 2000;
	[Export] private Node2D _spawnPointsRoot;
	private Timer _spawnTimer;
	private Vector2 _targetPosition;
	private Vector2[] _spawnPoints;
	private readonly List<Vector2[]> _paths = [];
	private int _amountEnemies;
	private bool _enabled;

	public void Initialize()
	{
		InitializeTarget();
		InitializeSpawnPoints();
		InitializeSpawnTimer();

		for (int i = 0; i < _spawnPoints.Length; i++)
		{
			_paths.Add([]);
		}
	}

	public void SetEnabled(bool enabled)
	{
		_enabled = enabled;
	}

	private void InitializeSpawnPoints()
	{
		var children = _spawnPointsRoot.GetChildren();
		_spawnPoints = new Vector2[children.Count];
		for (int i = 0; i < _spawnPoints.Length; i++)
		{
			Node2D currentNode2D = children[i] as Node2D;
			if (currentNode2D == null)
			{
				//GD.PrintErr("Couldn't find Node2D in _spawnPointsRoot"); //Important!
				continue;
			}
			_spawnPoints[i] = currentNode2D.GlobalPosition;
		}
	}
    
	private void InitializeTarget()
	{
		Node2D target = GetTree().GetFirstNodeInGroup("TargetEnd") as Node2D;
;
		if(!IsInstanceValid(target))
		{
			//GD.PrintErr("No Available Targets found"); //Important!
			return;
		}
        //GD.Print("Target Position: " + target.GlobalPosition);
        
        _targetPosition = target.GlobalPosition;
	}

	private void InitializeSpawnTimer()
	{
		_spawnTimer = new Timer();
		_spawnTimer.WaitTime = _spawnDelay;
		AddChild(_spawnTimer);
		_spawnTimer.Timeout += OnSpawnTimeout;
		_spawnTimer.Start();
	}

	private void TryToGetPathIfEmpty()
	{
		for (int i = 0; i < _spawnPoints.Length; i++)
		{
			Vector2[] currentPath = _paths[i];
			
			if(currentPath.Length != 0)
				continue;
			
			_paths[i] = GetNavigationPath(_spawnPoints[i], _targetPosition);
		}
	}
    
	public override void _PhysicsProcess(double delta)
	{
		if (!_enabled)
			return;
		TryToGetPathIfEmpty(); 
	}

	private void OnSpawnTimeout()
	{
		if (!_enabled)
			return;
		//GD.Print("Spawning Enemy...");
		_amountEnemies = GetChildren().Count;
		//GD.Print("Amount of enemies: " + _amountEnemies);

		if (_amountEnemies + _spawnPoints.Length > _maxEnemies)
		{
			//GD.Print("Maximum amount of enemies reached... aborting");
			return;
		}
		
		for(int i = 0; i < _spawnPoints.Length; i++)
		{
			Vector2 spawnPoint = _spawnPoints[i];
			Vector2[] path = _paths[i];
			
			if (path.Length == 0)
			{
				//GD.Print("_path is empty, aborting..."); //Important!
				return;
			}

			//GD.Print("Enemy Spawned At: ", spawnPoint);
			//GD.Print("Target Position: ", _targetPosition);
			
			Enemy enemyInstance = _enemyScene.Instantiate<Enemy>();
			enemyInstance.GlobalPosition = spawnPoint + new Vector2(GD.RandRange(-5, 5), GD.RandRange(-5, 5));
			enemyInstance.SetPath(path);
			AddChild(enemyInstance);
		}
	}
	
	private Vector2[] GetNavigationPath(Vector2 startPosition, Vector2 targetPosition)
	{
		 /*if (!IsInsideTree())
		 {
		 	return Array.Empty<Vector2>();
		 }*/

		Rid defaultMapRid = GetWorld2D().NavigationMap;
		Vector2[] path = NavigationServer2D.MapGetPath(
			defaultMapRid,
			startPosition,
			targetPosition,
			true,
			2
		);
		
		return path;
	}
}