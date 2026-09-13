using System;
using System.Collections.Generic;
using Godot;
using Towerdefense.combat.enemy;

namespace Towerdefense.systems;

public partial class EnemyManager : Node
{
	private readonly List<EnemyData> _enemies = new List<EnemyData>();
	private readonly List<Vector2[]> _paths = new List<Vector2[]>();
	
	private Timer _tickTimer;

	public override void _Ready()
	{
		InitializeTickTimer();
	}

	private void InitializeTickTimer()
	{
		_tickTimer = new Timer();
		_tickTimer.WaitTime = 1;
		AddChild(_tickTimer);
		_tickTimer.Timeout += OnTick;
		_tickTimer.Start();
	}
	
	public void SetPathsData(List<Vector2[]> pathsData)
	{
		_paths.Clear();
		for (int i = 0; i < pathsData.Count; i++)
		{
			_paths.Add(pathsData[i]);
		}
	}
	
	public void AddEnemy(EnemyData enemyData)
	{
		_enemies.Add(enemyData);
	}

	private float CalculatePathLength(Vector2[] pathData)
	{
		float totalLength = 0;

		for (int i = 1; i < pathData.Length; i++)
		{
			Vector2 prevVector =  pathData[i - 1];
			Vector2 currentVector = pathData[i];
			
			totalLength += prevVector.DistanceTo(currentVector);
		}
		
		return totalLength;
	}

	private Vector2 GetPositionInPathFromPercentage(Vector2[] pathData, float percentage)
	{
		float[] percentageData = new float[pathData.Length];

		for (int i = 0; i < percentageData.Length; i++)
		{
			percentageData[i] = (float)(i) / (percentageData.Length - 1);
			//GD.Print("% Data: "  + percentageData[i]);
		}

		int closestVectorIndex = 0;
		float smallestDelta = MathF.Abs(percentage - percentageData[0]);
		for (int i = 0; i < percentageData.Length; i++)
		{
			float currentPercentage = percentageData[i];
			float currentDelta = MathF.Abs(percentage - currentPercentage);
			if (percentage < currentPercentage) //important
				continue;

			if (currentDelta < smallestDelta)
			{
				smallestDelta = currentDelta;
				closestVectorIndex = i;
			}
		}

		int v2Index = Math.Min(pathData.Length - 1, closestVectorIndex + 1);
		
		Vector2 v1 = pathData[closestVectorIndex];
		Vector2 v2 = pathData[v2Index];

		// if (v2 == v1)
		// {
		// 	v2Index = closestVectorIndex - 1;
		// 	v2 = pathData[v2Index];
		// }
		
		// GD.Print("v1: " + v1);
		// GD.Print("v2: " + v2);

		Vector2 delta = v2 - v1;
		float v2Percentage = percentageData[v2Index] * 100;
		float closestPercentage = percentageData[closestVectorIndex] * 100;
		// GD.Print("v2Percentage: " + v2Percentage);
		// GD.Print("closestPercentage: " + closestPercentage);
		
		float percentageDelta = v2Percentage - closestPercentage;
		float relativeValue = percentage * 100 - closestPercentage;
		float relativePercentage = relativeValue / percentageDelta;  

		// GD.Print("RelativePercentage: " + relativePercentage);
		// GD.Print("RelativePercentage =  " + relativeValue + " / " + percentageDelta);
		
		return v1 + delta * relativePercentage;
	}
	
	private void OnTick()
	{
		for (int i = 0; i < _enemies.Count; i++)
		{
			EnemyData currentEnemy = _enemies[i];
			Node2D enemyWorldInstance = currentEnemy.WorldInstance;
			GD.Print("Tick: " + currentEnemy.Tick);
			Vector2[] currentPath = _paths[currentEnemy.PathId];
			float currentPathLength =  CalculatePathLength(currentPath) / 20;
			GD.Print("Path Length: " + currentPathLength);
			
			GD.Print("Percentage: " + currentEnemy.Tick / currentPathLength);
			Vector2 randomDelta = new Vector2(GD.Randf() * 10, GD.Randf() * 10);
			enemyWorldInstance.GlobalPosition = GetPositionInPathFromPercentage(currentPath, currentEnemy.Tick / currentPathLength) + randomDelta;
			currentEnemy.Tick++;
		}
	}
}