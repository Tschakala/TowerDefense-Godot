using Godot;

namespace Towerdefense.source.world;

public class PathNode(Vector2I position)
{
    public Vector2I Position { get; set; } = position;
}