using Godot;

namespace Towerdefense.scripts;

public class PathNode(Vector2I position)
{
    public Vector2I Position { get; set; } = position;
}