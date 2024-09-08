using Godot;
using System;

public partial class TestTimer : Timer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Connectes Timeout Node with Signal (Listener)
		// Timeout is signal name
		Timeout += OnTimerTimeoutSignal;
	}

	// Signal Method for every time signal is broadcasted
	private void OnTimerTimeoutSignal()
	{
		GD.Print("Timer timeout");
	}
}
