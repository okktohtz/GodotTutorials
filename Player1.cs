using Godot;
using System;

public partial class Player1 : CharacterBody2D
{
	[Export]
	public float Speed = 150.0f;
	private int health = 100;
	private Vector2 spawnPosition;
	private bool isDead = false;
	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector(
			"ui_left",
			"ui_right",
			"ui_up",
            "ui_down"
		);

		Velocity = direction * Speed;

		MoveAndSlide();
	}
	
	public void TakeDamage(int damage)
	{
		if (isDead)
			return;

		health -= damage;

		GD.Print("Player HP: " + health);

		if (health <= 0)
		{
			Die();
		}
	}

	private async void Die()
	{
		isDead = true;
		
		GD.Print("Player died!");

		await ToSignal(
			GetTree().CreateTimer(2.0),
			SceneTreeTimer.SignalName.Timeout
		);

		Respawn();
	}

	private void Respawn()
	{
		Position = spawnPosition;
		health = 100;
		isDead = false;
		GD.Print("Player respawned!");
	}
	
	public override void _Ready()
	{
		spawnPosition = Position;
		
		GD.Print($"Player's Spawn: {spawnPosition} ");
	}

}
