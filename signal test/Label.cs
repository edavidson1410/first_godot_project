using Godot;
using System;

public partial class Label : Godot.Label
{
	// reference to timer
	[Export] private Timer _timer;
	[Export] private int _limit;
	private int _count = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// utilize reference to hear signal
		_timer.Timeout += OnTimerTimeoutSignal;
	}

	// Method that runs when label hears timeout signal
	private void OnTimerTimeoutSignal() {
		_count++;
		Text = _count.ToString();
		// if count is equal to limit, stop listening to signal
		// += listen
		// -= stop listening
		if(_count == _limit) {
			_timer.Timeout -= OnTimerTimeoutSignal;
		}
	}
}
