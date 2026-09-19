using System.Threading;
using Godot;

namespace Towerdefense.source;

public partial class DebugDataReader : Node
{
	private Node2D _enemyRoot;
	private int _enemyAmount;
	
	[Signal] private delegate void DebugDataChangedEventHandler(string data);
	
	public override void _Ready()
	{
		_enemyRoot = GetTree().GetFirstNodeInGroup("spawner_manager") as Node2D;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		_enemyAmount = _enemyRoot.GetChildCount() - 1;
		double fps = Engine.GetFramesPerSecond();
		double frameTime = 1f / fps;
		double deltaTime = delta;

		string finalData = $"FPS: {(int)fps}\nFrame Time: {(float)frameTime}\nEnemy Amount: {(float)_enemyAmount}";
		EmitSignal(SignalName.DebugDataChanged, finalData);
	}
}