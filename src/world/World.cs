using System;
using System.Collections.Generic;
using Godot;
using Towerdefense.systems;

namespace Towerdefense.world;

public partial class World : Node2D
{
	// TileMaps
	[Export] private TileMapLayer _backGround;
	[Export] private TileMapLayer _walls;
	[Export] private TileMapLayer _path;
	
	// Size
	[Export] private int _mapSize = 200;
	[Export] private int _pathcount = 15;
	[Export] private int _pathlenght = 3; // 1 == 3x3 paths, 2 == 5x5 paths ...
	[Export] private int _minDistanceToEnd = 850;
	
	// End
	private PathNode _endNode;
	[Export] private PackedScene _endScene;
	
	// Spawner
	private List<PathNode> _spawners = new();
	[Export] private PackedScene _spawnerScene;
	
	//SpawnerManager
	[Export] private SpawnerManager _spawnerManager;
	[Export] private Node2D _spawnPointsRoot;
	
	// Path
	private HashSet<Vector2I> _pathTiles = new();
	
	// Random Number
	private RandomNumberGenerator _rng = new RandomNumberGenerator();
	
	// NagivationRegion
	[Export] private NavigationRegion2D _nagivation;
	
	public override void _Ready()
	{
		_endNode = new PathNode(Vector2I.Zero);
		GenerateSpawners();
		
		GenerateBackground();
		GeneratePaths();
		DrawPaths();
		GenerateMap();

		_nagivation.BakeNavigationPolygon();
		
		foreach (PathNode n in _spawners)
		{
			GD.Print(n.GetPosition);
		}

		EnableAndInitializeSpawnerManager();
	}

	private void EnableAndInitializeSpawnerManager()
	{
		_spawnerManager.Initialize();
		_spawnerManager.SetEnabled(true);
	}
	
	private void GenerateBackground()
	{
		for (int i = -(_mapSize / 2); i < _mapSize; i++)
		{
			for (int j = -(_mapSize / 2); j < _mapSize; j++)
			{
				DrawColumn(_backGround,new Vector2I(i, j), 0, new Vector2I(5, 1));
			}
		}
	}
	
	private void GenerateMap()
	{
		for (int i = -(_mapSize / 2); i < _mapSize; i++)
		{
			for (int j = -(_mapSize / 2); j < _mapSize; j++)
			{
				if ((i == -(_mapSize / 2) || i == _mapSize - 1 || j == -(_mapSize / 2) || j == _mapSize - 1) || (i == -(_mapSize / 2) + 1 || i == _mapSize - 2 || j == -(_mapSize / 2) + 1 || j == _mapSize - 2)) // walls around the map
				{
					DrawBarrior(i, j);
				}
			}
		}
	}

	private void DrawBarrior(int i, int j)
	{
		if (j ==  -(_mapSize / 2))
		{
			DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(_rng.RandiRange(9, 12), 1));
		}
		else if (j == _mapSize - 1)
		{
			DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(_rng.RandiRange(9, 12), 6));
		}
		else if (i == -(_mapSize / 2))
		{
			DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(8, _rng.RandiRange(2, 5)));
		}
		else if (i == _mapSize - 1)
		{
			DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(13, _rng.RandiRange(2, 5)));
		}
		else if (i == -(_mapSize / 2) + 1)
		{
			DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(6, _rng.RandiRange(2, 5)));
		}
		else if (i == _mapSize - 2)
		{
			DrawColumn(_walls, new Vector2I(i, j), 3, new Vector2I(1, _rng.RandiRange(2, 5)));
		}
		else if (j ==  -(_mapSize / 2) + 1)
		{
			DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(_rng.RandiRange(2, 5), 6));
		}
		else if (j == _mapSize - 2)
		{
			DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(_rng.RandiRange(2, 5), 1));
		}
		
		if (j ==  -(_mapSize / 2))
		{
			if (i == -(_mapSize / 2))
			{
				DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(8, 1));
			}
			else if (i == _mapSize - 1)
			{
				DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(13, 1));
			}
		}
		else if (j == _mapSize - 1)
		{
			if (i == -(_mapSize / 2))
			{
				DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(8, 6));
			}
			else if (i == _mapSize - 1)
			{
				DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(13, 6));
			}
		}
		else if (j == -(_mapSize / 2) + 1)
		{
			if (i == -(_mapSize / 2) + 1)
			{
				DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(9, 2));
			}
			else if (i == _mapSize - 2)
			{
				DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(12, 2));
			}
		}
		else if (j == _mapSize - 2)
		{
			if (i == -(_mapSize / 2) + 1)
			{
				DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(9, 5));
			}
			else if (i == _mapSize - 2)
			{
				DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(12, 5));
			}
		}
	}
	
	private void GenerateSpawners()
	{
		for (int x = 0; x < _pathcount; x++)
		{
			Vector2I pos = new Vector2I((_rng.RandiRange(-(_mapSize / 2), _mapSize)) * 7, (_rng.RandiRange(-(_mapSize / 2), _mapSize)) * 7);
			while (pos.DistanceTo(_endNode.GetPosition) < _minDistanceToEnd)
			{
				pos = new Vector2I((_rng.RandiRange(-(_mapSize / 2), _mapSize)) * 7, (_rng.RandiRange(-(_mapSize / 2), _mapSize)) * 7);
			}
			PathNode node = new PathNode(pos);
			_spawners.Add(node);
			
			var spawnerInstance = _spawnerScene.Instantiate<Node2D>();
			spawnerInstance.GlobalPosition = node.GetPosition;
			_spawnPointsRoot.AddChild(spawnerInstance);
		}
		/*TargetEnd targetEndInstance = _endScene.Instantiate<TargetEnd>();
		targetEndInstance.GlobalPosition = _endNode.GetPosition;
		AddChild(targetEndInstance);*/
	}

	private void GeneratePaths()
	{
		foreach (PathNode node in _spawners)
		{
			Vector2I start = new Vector2I((int)(node.GetPosition.X / 7.7f), (int)(node.GetPosition.Y / 7.7f));
			Vector2I end = new Vector2I((int)(_endNode.GetPosition.X / 7.7f), (int)(_endNode.GetPosition.Y / 7.7f));

			CreatePath(start, end);
		}
	}
	
	private void CreatePath(Vector2I start, Vector2I end)
	{
		Vector2I current = start;

		if (current.X != end.X)
		{
			current.X += Math.Sign(end.X - current.X);
		}
		else if (current.Y != end.Y)
		{
			current.Y += Math.Sign(end.Y - current.Y);
		}
			

		while (current != end)
		{
			_pathTiles.Add(current);

			bool moveX = _rng.Randf() < 0.5f;

			if (moveX && current.X != end.X)
			{
				current.X += Math.Sign(end.X - current.X);
			}
			else if (current.Y != end.Y)
			{
				current.Y += Math.Sign(end.Y - current.Y);
			}
		}

		_pathTiles.Add(end);
	}

	private void DrawPaths()
	{
		foreach (Vector2I tile in _pathTiles)
		{
			for (int i = -_pathlenght; i <= _pathlenght; i++)
			{
				for (int j = -_pathlenght; j <= _pathlenght; j++)
				{
					DrawColumn(_path, tile + new Vector2I(i ,j), 1, new Vector2I(16,  5));
				}
			}
		}
	}
	
	private void DrawColumn(TileMapLayer layer, Vector2I pos, int idMap, Vector2I idTile)
	{
		layer.SetCell(pos, idMap, idTile);
	}
}