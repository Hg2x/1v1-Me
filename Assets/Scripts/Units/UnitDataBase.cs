using UnityEngine;
using static ICKT.Editor.EditorLibrary;

public abstract class UnitDataBase : ScriptableObject
{
	[Header("Base Unit Related")]
	[Min(0)] public float MaxHealth = 10;
	[ReadOnlyField] public float Health;
	[Min(0)] public float Attack = 1;
	[ReadOnlyField] public bool IsFacingRight;
	[Min(0)] public float BaseMoveSpeed;
	[ReadOnlyField] public bool IsAttacking = false;
	public float GravityScale;
	[Min(0)] public float XTolerance = 0.01f;
	[Min(0)] public float GroundedTolerance = 0.001f;

	public delegate void HealthChangeHandler(float health, float maxHealth);
	public event HealthChangeHandler OnHealthChanged;

	public virtual void ResetData()
	{
		Health = MaxHealth;
		IsAttacking = false;
		IsFacingRight = true;
	}

	public void ModifyHealth(float amount)
	{
		var tempHealth = Health;
		Health += amount;
		if (Health > MaxHealth)
		{
			Health = MaxHealth;
		}
		if (tempHealth != Health)
		{
			OnHealthChanged?.Invoke(Health, MaxHealth);
		}
	}

	public virtual float GetAttack()
	{
		return Attack;
	}
}
