using Godot;

namespace Towerdefense.combat.enemy;

public partial class BasicEnemy : RigidBody2D
{
	[Export] private float _acceleration = 20f;
    [Export] private float _friction = 700f;
    [Export] private bool _faceDirection = true;
    [Export] private Sprite2D _sprite;
    [Export] private NavigationAgent2D _navigationAgent2D;

    public override void _Ready()
    {
        _navigationAgent2D.VelocityComputed += OnVelocityComputed;
    }

    public void SetTargetPosition(Vector2 position)
    {
        _navigationAgent2D.SetTargetPosition(position);
        GD.Print("Set Target Position is successful");
    }
    
    private void SetSpriteFaceSide(short side)
    {
        if (side > 0)
        {
            if (_sprite.Scale.X < 0)
            {
                _sprite.Scale = new Vector2(-_sprite.Scale.X, _sprite.Scale.Y);
            }
        }
        else
        {
            if (_sprite.Scale.X > 0)
            {
                _sprite.Scale = new Vector2(-_sprite.Scale.X, _sprite.Scale.Y);
            }
        }
    }

    private void TrySetSpriteFace(Vector2 direction)
    {
        if(!_faceDirection || !IsInstanceValid(_sprite))
            return;
        
        if (direction.X == 0)
            return;
		
        if(direction.X > 0)
        {
            SetSpriteFaceSide(1);
            return;
        }
		
        if (direction.X < 0)
        {
            SetSpriteFaceSide(-1);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if(_navigationAgent2D.IsNavigationFinished())
            return;
        
        Vector2 nextPos = _navigationAgent2D.GetNextPathPosition();
        Vector2 direction = GlobalPosition.DirectionTo(nextPos);

        _navigationAgent2D.SetVelocity(direction * _acceleration);
        
        //TrySetSpriteFace(Velocity);
        //MoveAndSlide();
        
        GD.Print("Moved successfully");
    }

    private void OnVelocityComputed(Vector2 safeVelocity)
    {
        //Velocity = safeVelocity;
        ApplyForce(safeVelocity);
        GD.Print("Velocity is computed successfully");
    }

    private void OnDeath(string name)
    {
        this.QueueFree();
    }
}