using Godot;
using System;

public partial class player : CharacterBody2D
{
	[Export]
	public float speed = 200.0f;
	[Export]
	public float jumpVelocity = -200.0f;
	[Export]
	public float doubleJumpVelocity = -100.0f;

	
	AnimatedSprite2D playerAnimations = null;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private bool hasDoubleJump = false;
	private bool animation_locked = false;

	public override void _Ready()
	{
		playerAnimations = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;
		else
			hasDoubleJump = false;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump")){
			if(IsOnFloor()) {
				velocity.Y = jumpVelocity;
			} 
			else if (hasDoubleJump != true) {
				velocity.Y = doubleJumpVelocity;
				hasDoubleJump = true;
			}

		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * speed;
			playerAnimations.Play("run");
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			playerAnimations.Play("idle");

		}

		Velocity = velocity;
		MoveAndSlide();
		// flips animation
		bool isLeft = velocity.X < 0;
		playerAnimations.FlipH = isLeft;
	}
 
}
