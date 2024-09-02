using Godot;
using System;

public partial class Player : CharacterBody2D
{

	// public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	[Export]
	private float gravity = 1000;
	[Export]
	private int speed = 400;
	private bool jumping = false;
	[Export]
	private int jumpHeight = 500;


	public Vector2 ScreenSize;
	AnimatedSprite2D animatedSprite2D = null;
	public Vector2 velocity = Vector2.Zero; // The player's movement vector.



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		
		// add gravity
		if(IsOnFloor() != true) {
			velocity.Y += gravity * (float)delta;
		}
		// Jump reset
		if (jumping == true && IsOnFloor() == true)
        {
            jumping = false;
        }

		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * speed;
			if(jumping == false)	animatedSprite2D.Play("run");
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			animatedSprite2D.Play("idle");
		}

		// Jump logic
		if(Input.IsActionJustPressed("jump")) {
			Jump();
		}

		Velocity = velocity;
		MoveAndSlide();
		Flip_animation();
	}

	private void Flip_animation() {
		bool isLeft = velocity.X < 0;
		animatedSprite2D.FlipH = isLeft;
	}

	private void Jump() {
		jumping = true;
		velocity.Y = -jumpHeight;
		animatedSprite2D.Play("jump_loop");
	}
}
