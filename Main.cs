using Godot;
using System;
using System.Linq.Expressions;
using System.Xml.Resolvers;

public partial class Main : Node
{
	// allows us to choose the Mob scene we want to instance
	[Export]
	public PackedScene MobScene {get; set;}
	public static int score {get; set;}




	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		NewGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// timer test
		var label = GetNode<Label>("Label");
		label.Text = score.ToString();

	}

	public void GameOver() {
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
	}

	public void NewGame(){
		score = 0;

		var player = GetNode<PlayerController>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();

		Boar mob = MobScene.Instantiate<Boar>();

		// Choose a random location on Path2D.
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		// Set the mob's position to a random location.
		mob.Position = mobSpawnLocation.Position;

		// Spawn the mob by adding it to the Main scene.
		AddChild(mob);
	}

	private void OnMobTimerTimeout()
	{
		// Note: Normally it is best to use explicit types rather than the `var`
		// keyword. However, var is acceptable to use here because the types are
		// obviously Mob and PathFollow2D, since they appear later on the line.

		// Create a new instance of the Mob scene.
		Boar mob = MobScene.Instantiate<Boar>();

		// Choose a random location on Path2D.
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		// Set the mob's direction perpendicular to the path direction.
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

		// Set the mob's position to a random location.
		mob.Position = mobSpawnLocation.Position;

		// Add some randomness to the direction.
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;

		// Choose the velocity.
		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);

		// Spawn the mob by adding it to the Main scene.
		AddChild(mob);
	}
}
