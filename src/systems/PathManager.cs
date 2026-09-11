using Godot;

namespace Towerdefense.core;

public partial class PathManager : Node2D
{
	[Export] private NavigationAgent2D _agent;
	[Export] private Marker2D _start;
	private Marker2D _target;

	public Vector2[] Path { get; private set; }

	public override async void _Ready()
	{
		_target = GetTree().GetFirstNodeInGroup("TargetEnd") as Marker2D;
		if (!IsInstanceValid(_target))
		{
			GD.PrintErr("No Available Targets found");
			return;
		}
		
		_agent.SetTargetPosition(_target.GlobalPosition);

		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		
		
		Path = _agent.GetCurrentNavigationPath();
		
		//GD.Print(_agent.TargetPosition);
		GD.Print("Map: ", _agent.GetNavigationMap().IsValid);
		GD.Print("Reachable: ", _agent.IsTargetReachable());
		GD.Print("Path Length: ", Path.Length);
		
		GD.Print("Start: ", _start.GlobalPosition);
		GD.Print("Target: ", _target.GlobalPosition);
		GD.Print(_agent.GetNavigationMap());

		GD.Print("Closest Start: ",NavigationServer2D.MapGetClosestPoint(_agent.GetNavigationMap(), _start.GlobalPosition));
		GD.Print("Closest Target: ",NavigationServer2D.MapGetClosestPoint(_agent.GetNavigationMap(), _target.GlobalPosition));

		foreach (Vector2 point in Path)
		{
			GD.Print(point);
		}
	}   
}
