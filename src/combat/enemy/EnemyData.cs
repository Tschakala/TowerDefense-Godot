using Godot;

namespace Towerdefense.combat.enemy;

public class EnemyData(int pathId, Node2D worldInstance)
{
    public int PathId { get; } = pathId;
    public Node2D WorldInstance { get; } = worldInstance;
    public int Tick { get; set; }
}