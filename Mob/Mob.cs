using Godot;
using System;

public class Mob : RigidBody2D
{
    [Export]
    public int MinMovementSpeed = 150; 
    [Export]
    public int MaxMovementSpeed = 250;

    static private Random _random = new Random();

    public override void _Ready()
    {
        var sprite = GetNode<AnimatedSprite>("AnimatedSprite");
        var mobTypes = sprite.Frames.GetAnimationNames();
        sprite.Animation = mobTypes[_random.Next(0, mobTypes.Length)];
    }

    public void _on_VisibilityNotifier2D_screen_exited()
    {
        QueueFree();
    }
}
