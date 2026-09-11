using Godot;
using System;

public partial class World : Node2D
{
	// TileMaps
	[Export] private TileMapLayer _backGround;
	[Export] private TileMapLayer _walls;
	[Export] private TileMapLayer _path;
	
	// Size
	[Export] private int _mapSize = 100;
	
	// noise
	private FastNoiseLite _noise =  new FastNoiseLite();
	[Export] private float _frequence  = 0.02f;
	
	//random Number
	private RandomNumberGenerator _rng = new RandomNumberGenerator();
	
	public override void _Ready()
	{
		GenerateBackground();		
		GenerateMap();
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
		_noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
		_noise.Frequency = _frequence;
		

		for (int i = -(_mapSize / 2); i < _mapSize; i++)
		{
			for (int j = -(_mapSize / 2); j < _mapSize; j++)
			{
				float noiseValue = _noise.GetNoise2D(i, j);
				noiseValue = (noiseValue + 1) / 2; //Normalize from [-1;1] to [0;1]
				//GD.Print(noiseValue);
				if ((i == -(_mapSize / 2) || i == _mapSize - 1 || j ==  -(_mapSize / 2) || j == _mapSize - 1) || (i == -(_mapSize / 2) + 1 || i == _mapSize - 2 || j ==  -(_mapSize / 2) + 1 || j == _mapSize - 2)) // walls around the map
				{
					if (i == -(_mapSize / 2) + 1)
					{
						DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(6, _rng.RandiRange(2, 5)));
					}
					else if (i == _mapSize - 2)
					{
						DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(1, _rng.RandiRange(2, 5)));
					}
					else if (j ==  -(_mapSize / 2) + 1)
					{
						DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(_rng.RandiRange(2, 5), 6));
					}
					else if (j == _mapSize - 2)
					{
						DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(_rng.RandiRange(2, 5), 1));	
					}
					else if (i == -(_mapSize / 2))
					{
						DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(8, _rng.RandiRange(2, 5)));
					}
					else if (i == _mapSize - 1)
					{
						DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(13, _rng.RandiRange(2, 5)));
					}
					else if (j ==  -(_mapSize / 2))
					{
						DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(_rng.RandiRange(9, 12), 1));
					}
					else
					{
						DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(_rng.RandiRange(9, 12), 6));
					}
				}
				else
				{
					if (noiseValue >= 0.5)
					{
						if (noiseValue >= 0.5 && noiseValue < 0.52)
						{
							DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(3, 1));	
						}
						else if (noiseValue >= 0.52 && noiseValue < 0.54)
						{
							DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(3, 2));						
						}
						else
						{ 
							DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(28, 3));	
						}
					}
					else
					{
						if (j % 2 == 0)
						{
							DrawColumn(_path,new Vector2I(i,j),1, new Vector2I(16, 5));	
						}
						else
						{
							DrawColumn(_path,new Vector2I(i,j),1, new Vector2I(16, 6));
						}
					
					}
				}
			}
		}
		
	}

	private void DrawColumn(TileMapLayer layer, Vector2I pos, int idMap, Vector2I idTile)
	{
		layer.SetCell(pos, idMap, idTile);
	}
}
