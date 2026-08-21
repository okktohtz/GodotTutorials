using Godot;
using System;
using Tutorial2;

public partial class Player1 : CharacterBody2D
{
	[Export]
	public float Speed = 150.0f;

	private int health = 100;
	private Vector2 spawnPosition;
	private PlayerState CurrentState = PlayerState.Normal;
    private bool isFlashing = false;
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
		if (CurrentState == PlayerState.Dead)
		{
			GlobalHelper.Log("Damage ignored - Player is dead");
			return;
		}

		if (CurrentState == PlayerState.Invincible)
		{
			GlobalHelper.Log("Damage ignored - Player is invincible");
			return;
		}

		health -= damage;

		GlobalHelper.Log("Player HP: " + health);

		CurrentState = PlayerState.Invincible;

		GlobalHelper.Log("Player is now invincible");

		GetNode<Timer>("DamageCoolDown").Start();

		GlobalHelper.Log("Damage cooldown started");

        GetNode<Timer>("InvincibilityFlashTimer").Start();


        if (health <= 0)
		{
			Die();
		}
	}

	private async void Die()
	{
		CurrentState = PlayerState.Dead;

		GlobalHelper.Log("Player died!");

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

		CurrentState = PlayerState.Respawning;

		GlobalHelper.Log("Player respawned!");

		CurrentState = PlayerState.Normal;

		GlobalHelper.Log("Player state changed to Normal");
	}

	public override void _Ready()
	{
		spawnPosition = Position;

		GlobalHelper.Log($"Player's Spawn: {spawnPosition}");
	}

	private void OnDamageCooldownTimeout()
	{
        CurrentState = PlayerState.Normal;

        GetNode<Timer>("InvincibilityFlashTimer").Stop();

        isFlashing = false;
        Modulate = new Color(1, 1, 1, 1);

        GlobalHelper.Log("Damage cooldown ended - Player is vulnerable");
    }

    private void OnInvincibilityFlashTimerTimeout()
    {
        isFlashing = !isFlashing;

        Modulate = isFlashing
            ? new Color(1, 1, 1, 0.3f)
            : new Color(1, 1, 1, 1);
    }
}
