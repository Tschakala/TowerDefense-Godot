using System;
using System.Threading.Tasks;
using Godot;
using Towerdefense.combat.enemy;

namespace Towerdefense.systems;

public partial class Spawner : Node2D
{
    [Export] private PackedScene _enemyScene;
    [Export] private float _spawnDelay = 0.05f; //In Seconds
    private Timer _spawnTimer = new Timer();
    private Vector2[] _path;
    private Vector2 _targetPosition;
	
    public override void _Ready()
    {
        InitializeTarget();
        InitializeSpawnTimer();
    }
    
    private void InitializeTarget()
    {
        Node2D target = GetTree().GetFirstNodeInGroup("TargetEnd") as Node2D;
        if(!IsInstanceValid(target))
        {
            GD.PrintErr("No Available Targets found"); //Wichtig!
            return;
        }
        
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
        if(!_path.IsEmpty())
            return;
        
        _path = GetNavigationPath(GlobalPosition, _targetPosition);
    }
    
    public override void _PhysicsProcess(double delta)
    {
        TryToGetPathIfEmpty(); 
    }

    private void OnSpawnTimeout()
    {
        if(_path.Length == 0)
        {
            GD.Print("_path is empty, aborting..."); //Wichtig!
            return;
        }
        
        Enemy enemyInstance = _enemyScene.Instantiate<Enemy>();
        AddChild(enemyInstance);
        enemyInstance.GlobalPosition = GlobalPosition +  new Vector2(GD.RandRange(-5, 5), GD.RandRange(-5, 5));
        enemyInstance.SetPath(_path);
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
		
        return path;
    }
}