using Godot;
using System;

public partial class player : CharacterBody2D
{
	[Export]
	public float speed = 200.0f;
	[Export]
	public float jumpVelocity = -300.0f;
	[Export]
	public float doubleJumpVelocity = -200.0f;

	
	AnimatedSprite2D playerAnimations = null;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private bool hasDoubleJump = false;
	private bool animation_locked = false;
	private bool was_in_air = false;
	Vector2 velocity;
	Vector2 direction = Vector2.Zero;

	public override void _Ready()
	{
		playerAnimations = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{

		// Add the gravity.
		if (!IsOnFloor()) {
			velocity.Y += gravity * (float)delta;
			was_in_air = true;
		}
		else {
			hasDoubleJump = false;
			if(was_in_air) {
				Land();
			}
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump")){
			if(IsOnFloor()) {
				Jump();
			} 
			else if (hasDoubleJump != true) {
				Double_jump();
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
		// Update_animation();
		Flip_animation();
	}

	private void Update_animation() {
		if (animation_locked == false) {
			playerAnimations.Play("jump_loop");
		} else {
			if (direction.X != 0) playerAnimations.Play("run");
			else playerAnimations.Play("idle");
		}
	}

	private void Flip_animation() {
		bool isLeft = velocity.X < 0;
		playerAnimations.FlipH = isLeft;
	}

	private void Jump(){
		velocity.Y = jumpVelocity;
		playerAnimations.Play("jump_start");
		animation_locked = true;
	}

	private void Double_jump(){
		velocity.Y = doubleJumpVelocity;
		playerAnimations.Play("jump_double");
		animation_locked = true;
		hasDoubleJump = true;
	}

	private void Land() {
		playerAnimations.Play("jump_end");
		animation_locked = true;
	}

	private void _On_animated_sprite_2d_animation_finished() {

	}
}
