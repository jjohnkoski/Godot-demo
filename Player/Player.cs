using Godot;

public class Player : Area2D
{
	[Export]
	public int MovementSpeed = 400;
	[Signal]
	public delegate void Hit();

	private Vector2 _screenSize;

	public override void _Ready()
	{
		_screenSize = GetViewport().Size;
        Hide();
    }

	public void Start(Vector2 pos)
	{
		Position = pos;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	public override void _Process(float delta)
	{
		Vector2 velocity = new Vector2();
		var sprite = GetNode<AnimatedSprite>("AnimatedSprite");

		if (Input.IsActionPressed("ui_right"))
		{
			velocity.x += 1;
		}
		if (Input.IsActionPressed("ui_left"))
		{
			velocity.x -= 1;
		}
		if (Input.IsActionPressed("ui_down"))
		{
			velocity.y += 1;
		}
		if (Input.IsActionPressed("ui_up"))
		{
			velocity.y -= 1;
		}

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * MovementSpeed;
			sprite.Play();
		}
		else
		{
			sprite.Stop();
		}

		Position += velocity * delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.x, 0, _screenSize.x),
			y: Mathf.Clamp(Position.y, 0, _screenSize.y)
		);

		if (velocity.x != 0)
		{
			sprite.Animation = "walk";
			sprite.FlipV = false;
			sprite.FlipH = velocity.x < 0;
		}
		else if (velocity.y != 0)
		{
			sprite.Animation = "up";
			sprite.FlipV = velocity.y > 0;
		}
	}

	public void _on_Player_body_entered(PhysicsBody2D body)
	{
		Hide(); 
		EmitSignal("Hit");
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
	}
}
