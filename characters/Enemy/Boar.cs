using Godot;
using System;

public partial class Boar : RigidBody2D
{
	// A signal can optionally declare one or more arguments. Specify the argument names between parentheses:
	// [Signal]
	// public delegate void HealthDepletedEventHandler();
	[Signal]
    public delegate void HealthChangedEventHandler(int oldValue, int newValue);
	private int _health = 10;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play("idle");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// deletes mob when off screen
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}

	// To emit values along with the signal, add them as extra arguments to the emit() function:
	public void TakeDamage(int amount) {
		int oldHealth = _health;
		_health -= amount;
		EmitSignal(SignalName.HealthChanged, oldHealth, _health);
	}
}
