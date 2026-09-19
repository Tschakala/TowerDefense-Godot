using Godot;
using Towerdefense.source.core;

namespace Towerdefense.source.combat.player;

public partial class Player : Node
{
	private static readonly Config Config = new Config();
	public int Health = Config.MaxHealth;
	private int _damage = 1;
	
	[Signal] public delegate void HealthChangedEventHandler (int newHealth);
	
	public override void _Ready()
	{
		
	}

	public override void _Process(double delta)
	{
		ChangeHealth(Health - _damage);
	}

	private void ChangeHealth(int newHealth)
	{
		EmitSignal(SignalName.HealthChanged, newHealth);
		Health = newHealth;
	}
}