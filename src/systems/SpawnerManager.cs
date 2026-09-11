using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Towerdefense.combat.enemy;

namespace Towerdefense.core;

public partial class SpawnerManager : Node2D
{
	[Export] private PackedScene _enemyScene;
	[Export] private float _spawnDelay = 0.05f; //In Seconds
	[Export] private int _maxEnemies = 600;
	[Export] private Node2D _spawnPointsRoot;
	private Timer _spawnTimer = new Timer();
	private Vector2 _targetPosition;
	private Vector2[] _spawnPoints;
	private List<Vector2[]> _paths = new List<Vector2[]>();
	private int _amountEnemies = 0;
	
	public override void _Ready()
	{
		InitializeTarget();
		InitializeSpawnTimer();
		InitializeSpawnPoints();

		for (int i = 0; i < _spawnPoints.Length; i++)
		{
			_paths.Add([]);
		}
	}

	private void InitializeSpawnPoints()
	{
		var children = _spawnPointsRoot.GetChildren();
		_spawnPoints = new Vector2[children.Count - 1];
		for (int i = 0; i < _spawnPoints.Length; i++)
		{
			Node2D currentNode2D = children[i] as Node2D;
			if (currentNode2D == null)
			{
				GD.PrintErr("Couldn't find Node2D in _spawnPointsRoot"); //Wichtig!
				continue;
			}
			_spawnPoints[i] = currentNode2D.GlobalPosition;
		}
	}
    
	private void InitializeTarget()
	{
		Node2D target = GetTree().GetFirstNodeInGroup("TargetEnd") as Node2D;
		if(!IsInstanceValid(target))
		{
			GD.PrintErr("No Available Targets found"); //Wichtig!
			return;
		}
        //GD.Print("Target Position: " + target.GlobalPosition);
        
        _targetPosition = target.GlobalPosition;
	}

	private void InitializeSpawnTimer()
	{
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
		TryToGetPathIfEmpty(); 
	}

	private void OnSpawnTimeout()
	{
		_amountEnemies = GetChildren().Count;
		GD.Print("Amount of enemies: " + _amountEnemies);

		if (_amountEnemies + _spawnPoints.Length > _maxEnemies)
		{
			GD.Print("Maximum amount of enemies reached... aborting");
			return;
		}
		
		for(int i = 0; i < _spawnPoints.Length; i++)
		{
			Vector2 spawnPoint = _spawnPoints[i];
			Vector2[] path = _paths[i];
			
			//GD.Print("Spawnpoint: " + spawnPoint);
			//GD.Print("Path: " + path.Length);
			
			if (path.Length == 0)
			{
				GD.Print("_path is empty, aborting..."); //Wichtig!
				return;
			}

			Enemy enemyInstance = _enemyScene.Instantiate<Enemy>();
			AddChild(enemyInstance);
			enemyInstance.GlobalPosition = spawnPoint + new Vector2(GD.RandRange(-5, 5), GD.RandRange(-5, 5));
			enemyInstance.SetPath(path);
		}
	}
	
	private Vector2[] GetNavigationPath(Vector2 startPosition, Vector2 targetPosition)
	{
		// if (!IsInsideTree())
		// {
		// 	return Array.Empty<Vector2>();
		// }

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