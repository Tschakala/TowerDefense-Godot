using Godot;

namespace Towerdefense.source.core;

public partial class Config : Node
{
    //Player
    [Export] public float Sensitivity = 0.25f;
    [Export] public float ZoomSensitivity = 0.07f;
    [Export] public int MaxHealth = 100;
    
    //Enemy
    [Export] public float MinSpeed = 22f;
    [Export] public float MaxSpeed = 28f;
    [Export] public int FpsBuffer = 5;
    
    //Spawner Manager
    [Export] public float SpawnDelay = 0.05f; //In Seconds
    [Export] public int MaxEnemies = 1000;
    
    //World
    [Export] public int MapSize = 200;
    [Export] public int PathCount = 15;
    [Export] public int PathLength = 3; // 1 == 3x3 paths, 2 == 5x5 paths ...
    [Export] public int MinDistanceToEnd = 100;
}