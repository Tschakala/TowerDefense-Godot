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
	
	public override void _Ready()
	{
		GenerateBackground();		
		GenerateMap();
	}

	private void GenerateBackground()
	{
		for (int i = 0; i < _mapSize; i++)
		{
			for (int j = 0; j < _mapSize; j++)
			{
				DrawColumn(_backGround,new Vector2I(i, j), 0, new Vector2I(8, 2));
			}
		}
	}
	
	private void GenerateMap()
	{
		_noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
		_noise.Frequency = _frequence;
		

		for (int i = 0; i < _mapSize; i++)
		{
			for (int j = 0; j < _mapSize; j++)
			{
				float noiseValue = _noise.GetNoise2D(i, j);
				// noiseValue += 1;
				// noiseValue /= 2;
				noiseValue = (noiseValue + 1) / 2; //Normalize from [-1;1] to [0;1]
				//GD.Print(noiseValue);

				if (noiseValue >= 0.5)
				{
					DrawColumn(_walls,new Vector2I(i,j),3, new Vector2I(3, 1));
				}
				else
				{
					DrawColumn(_path,new Vector2I(i,j),1, new Vector2I(3, 3));
				}
			}
		}
		
	}

	private void DrawColumn(TileMapLayer layer, Vector2I pos, int idMap, Vector2I idTile)
	{
		layer.SetCell(pos, idMap, idTile);
	}
}
