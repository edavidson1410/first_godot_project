using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void HitEventHandler();
	// public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	[Export]
	private float gravity = 1000;
	[Export]
	private int speed = 400;
	private bool attacking = false;
	private bool jumping = false;
	[Export]
	private int jumpHeight = 500;


	public Vector2 ScreenSize;
	AnimatedSprite2D animatedSprite2D = null;
	CollisionShape2D collisionShape2D = null;
	public Vector2 velocity = Vector2.Zero; // The player's movement vector.



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get Nodes
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		
		// add gravity
		if(IsOnFloor() != true) {
			velocity.Y += gravity * (float)delta;
		}
		// jump reset
		if (jumping == true && IsOnFloor() == true)
        {
            jumping = false;
        }
		// Jump logic
		if(Input.IsActionJustPressed("jump")) {
			Jump();
		}
		if(attacking == true && !Input.IsActionJustPressed("attack")) {
			attacking = false;
		}
		// Attack
		if(Input.IsActionPressed("attack")){
			Attack();
		}

		// running/idle
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * speed;
			if(jumping == false && attacking == false)	animatedSprite2D.Play("run");
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			if(jumping == false && attacking == false) animatedSprite2D.Play("idle");
		}



		Velocity = velocity;
		MoveAndSlide();
		Flip_animation();
	}

	private void Input_manager() {

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

	private void Attack() {
		attacking = true;
		animatedSprite2D.Play("attack");
	}

	private void OnBodyEntered(Node2D body)
	{
		Hide(); // Player disappears after being hit.
		EmitSignal(SignalName.Hit);
		// Must be deferred as we can't change physics properties on a physics callback.
		collisionShape2D.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
}
