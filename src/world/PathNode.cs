using System.Collections.Generic;
using Godot;
using System;

namespace Towerdefense.world;

public class PathNode
{
    private Vector2I _position;
    private List<PathNode> _children = new();
    private PathNode _parent;

    public Vector2I GetPosition
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
        }
    }

    public PathNode(Vector2I Position)
    {
        _position = Position;
    }
}