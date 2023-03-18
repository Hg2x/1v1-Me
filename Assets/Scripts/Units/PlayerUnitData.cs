using UnityEngine;
using static ICKT.Editor.EditorLibrary;

public class PlayerUnitData : UnitDataBase
{
	[Header("Player Unit Related")]
	[Min(0)] public float JumpForce;
	[Min(0)] public int JumpAmount;
	[ReadOnlyField] public int JumpAmountLeft;
	[Min(0)] public float DodgeDuration;
	[ReadOnlyField] public float DodgeDurationLeft = 0;
	[Min(0)] public float DodgeCooldown;
	[ReadOnlyField] public float DodgeCooldownLeft = 0;
	[Min(0)] public float DodgeSpeed;

	public override void ResetData()
	{
		base.ResetData();
		DodgeDurationLeft = 0;
		DodgeCooldownLeft = 0;
		JumpAmountLeft = JumpAmount;
	}

	public bool CanJump()
	{
		return JumpAmountLeft > 0;
	}

	public bool IsDodging()
	{
		return DodgeDurationLeft > 0;
	}

	public bool CanDodge()
	{
		return DodgeCooldownLeft <= 0;
	}
}
