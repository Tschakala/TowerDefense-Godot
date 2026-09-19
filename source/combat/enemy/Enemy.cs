using Godot;
using Towerdefense.source.core;

namespace Towerdefense.source.combat.enemy;

public partial class Enemy : CharacterBody2D
{
    private static readonly Config Conf = new Config();
    
    [Export] private AnimatedSprite2D _frameSprite;
    private float _minSpeed = Conf.MinSpeed;
    private float _maxSpeed = Conf.MaxSpeed;
    private int _fpsBuffer = Conf.FpsBuffer;
    private Vector2[] _path;
    private int _currentPathIndex;
    private float _speed;
    private int _frameCount;
    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        // if (_rng.RandiRange(1, 1000) == 999)
        // {
        //     _frameSprite.Frame = 0;
        // }
        // else
        // {
        //     _frameSprite.Frame = 14;
        // }
        
        _frameSprite.Frame = GD.RandRange(0, 14);
        _speed = (float)GD.RandRange(_minSpeed, _maxSpeed);
    }
    
    public void SetPath(Vector2[] path)
    {
        _path = path;
        _currentPathIndex = 0;
    }
    
    public override void _PhysicsProcess(double delta)
    {
        _frameCount++;

        if (_frameCount % _fpsBuffer == 0)
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

                if (_currentPathIndex >= _path.Length) 
                {
                    return;
                }

                targetPoint = _path[_currentPathIndex];
            }
            
            Vector2 direction = (targetPoint - GlobalPosition).Normalized();
            Velocity = direction * _speed * _fpsBuffer;
            
            MoveAndSlide();
            
            for (int i = 0; i < GetSlideCollisionCount(); i++)
            {
                var collision = GetSlideCollision(i);

                if (collision.GetCollider() is Enemy other)
                {
                    other.GlobalPosition += (Velocity.Normalized() * 1.5f) * _fpsBuffer;
                }
            }
        }
        
    }
}