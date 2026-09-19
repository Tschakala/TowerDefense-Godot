using Godot;
using Towerdefense.source.core;

namespace Towerdefense.source.combat.player;

public partial class Player : Node
{
	private static readonly Config Config = new Config();
	private int _health = Config.MaxHealth;
	private int _damage = 1;
	
	[Signal] public delegate void HealthChangedEventHandler (int newHealth);
	
	public override void _Ready()
	{
		
	}

	public override void _PhysicsProcess(double delta)
	{
		//ChangeHealth(_health - _damage);
	}

	private void ChangeHealth(int newHealth)
	{
		EmitSignal(SignalName.HealthChanged, newHealth);
		_health = newHealth;
	}
}