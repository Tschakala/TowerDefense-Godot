using Godot;
using Towerdefense.source.core;

namespace Towerdefense.source.combat.player;

public partial class HealthBar : ProgressBar
{
	private static readonly Config Config = new Config();
	
	private int _maxValue;
	
	public override void _Ready()
	{
		_maxValue = Config.MaxHealth;
		Value = Config.MaxHealth;
	}

	public override void _Process(double delta)
	{
		
	}

	public void OnHealthChanged(int newValue)
	{
		Value = newValue;
		GD.Print(Value);
	}
}