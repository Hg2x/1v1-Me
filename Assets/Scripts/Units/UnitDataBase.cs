using UnityEngine;
using static ICKT.Editor.EditorLibrary;

public abstract class UnitDataBase : ScriptableObject
{
	[Header("Base Unit Related")]
	[ReadOnlyField] public bool IsFacingRight;
	[Min(0)] public float BaseMoveSpeed;
	[ReadOnlyField] public bool IsAttacking = false;
	[Min(0)] public float XTolerance = 0.01f;
	[Min(0)] public float GroundedTolerance = 0.001f;

	public virtual void ResetData()
	{
		IsAttacking = false;
		IsFacingRight = true;
	}
}
