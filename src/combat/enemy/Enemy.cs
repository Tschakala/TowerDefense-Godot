using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Towerdefense.combat.enemy;

public partial class Enemy : CharacterBody2D
{
    [Export] private float _speed = 25f;
    private Vector2[] _path;
    private readonly List<Vector2> _reachedPoints = new List<Vector2>();
    
    public void SetPath(Vector2[] path)
    {
        _path = path;
        
        if(_path.Length == 0)
            return;
        
        GD.Print("[INFO] Path is successfully initialized");
    }

    Vector2 GetNextNearestPointInPath()
    {
        if (_path == null || _path.Length == 0)
        {
            GD.PrintErr("Path is null or empty");
            return Vector2.Zero;
        }
        
        Vector2 nearestPoint = _path[^1];
        for (int i = 0; i < _path.Length; i++)
        {
            Vector2 currentPoint = _path[i];
            if (_reachedPoints.Contains(currentPoint))
            {
                continue;
            }
            
            float currentDistance = GlobalPosition.DistanceTo(currentPoint);
            float distanceToNearestPoint = GlobalPosition.DistanceTo(nearestPoint);
            
            if (currentDistance < distanceToNearestPoint)
            {
                nearestPoint = currentPoint;
            }
        }
        
        return nearestPoint;
    }
    
    public override void _PhysicsProcess(double delta)
    {
        Vector2 nearestPoint = GetNextNearestPointInPath();
        Vector2 direction = nearestPoint - GlobalPosition;
        if (GlobalPosition.DistanceTo(nearestPoint) < 0.5f)
        {
            GD.Print("[INFO] Reached Point: " + nearestPoint);
            _reachedPoints.Add(nearestPoint);
            return;
        }
        
        GD.Print("[INFO] Moving to" + nearestPoint);
        //GD.Print(GlobalPosition.DistanceTo(nearestPoint));
        
        direction = direction.Normalized();

        Velocity = direction * _speed;
        MoveAndSlide();
    }
}