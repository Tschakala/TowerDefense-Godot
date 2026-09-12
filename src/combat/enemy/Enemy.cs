using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Towerdefense.combat.enemy;

public partial class Enemy : CharacterBody2D
{
    [Export] private float _minSpeed = 22f;
    [Export] private float _maxSpeed = 28f;
    private Vector2[] _path;
    private int _currentPathIndex = 0;
    [Export] private AnimatedSprite2D _frameSprite;

    public override void _Ready()
    {
        _frameSprite.Frame = GD.RandRange(0, 14);
    }
    
    public void SetPath(Vector2[] path)
    {
        _path = path;
        _currentPathIndex = 0;
    }
    
    public override void _PhysicsProcess(double delta)
    {
        if (_path == null || _path.Length == 0)
            return;
        
        while (_currentPathIndex < _path.Length - 1)
        {
            Vector2 current = _path[_currentPathIndex];
            Vector2 next = _path[_currentPathIndex + 1];

            if (GlobalPosition.DistanceTo(next) < GlobalPosition.DistanceTo(current))
            {
                _currentPathIndex++;
            }
            else
            {
                break;
            }
        }

        Vector2 targetPoint = _path[_currentPathIndex];
        
        if (GlobalPosition.DistanceTo(targetPoint) < 15f)
        {
            _currentPathIndex++;

            // if (_currentPathIndex >= _path.Length)
            // {
            //     QueueFree();
            //     return;
            // }

            targetPoint = _path[_currentPathIndex];
        }

        Vector2 direction = (targetPoint - GlobalPosition).Normalized();
        direction = direction.Normalized();
        Velocity = direction * (float)GD.RandRange(_minSpeed, _maxSpeed);
        
        MoveAndSlide();

        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);

            if (collision.GetCollider() is Enemy other)
            {
                other.GlobalPosition += Velocity.Normalized() * 1.5f;
            }
        }
    }
}