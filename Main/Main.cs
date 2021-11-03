using Godot;
using System;

public class Main : Node
{
    [Export]
    public PackedScene Mob;

    private int _score;
    private Random _random = new Random();

    public override void _Ready()
    {
        
    }

    private float RandRange(float min, float max)
    {
        return (float)_random.NextDouble() * (max - min) + min;
    }

    public void game_over()
    {
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();
        GetNode<HUD>("HUD").ShowGameOver();
        GetTree().CallGroup("mobs", "queue_free");
    }

    public void new_game()
    {
        _score = 0;

        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Position2D>("StartPosition");
        player.Start(startPosition.Position);

        var hud = GetNode<HUD>("HUD");
        hud.UpdateScore(_score);
        hud.ShowMessage("AH! CATS!");

        GetNode<Timer>("StartTimer").Start();
    }

    public void _on_StartTimer_timeout()
    {
        GetNode<Timer>("MobTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }

    public void _on_ScoreTimer_timeout()
    {
        _score++;
        GetNode<HUD>("HUD").UpdateScore(_score);
    }

    public void _on_MobTimer_timeout()
    {
        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.Offset = _random.Next();

        var mobInstance = (RigidBody2D)Mob.Instance();
        AddChild(mobInstance);

        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
        mobInstance.Position = mobSpawnLocation.Position;
        direction += RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mobInstance.Rotation = direction;
        mobInstance.LinearVelocity = new Vector2(RandRange(150f, 250f), 0).Rotated(direction);
    }
}
