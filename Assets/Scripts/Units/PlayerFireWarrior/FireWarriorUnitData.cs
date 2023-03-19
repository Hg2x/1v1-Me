using UnityEngine;
using static ICKT.Editor.EditorLibrary;

[CreateAssetMenu(fileName = "FireWarriorUnitData", menuName = "ScriptableObject/PlayerUnitData/FireWarriorUnitData")]
public class FireWarriorUnitData : PlayerUnitData
{
	[Header("Fire Warrior Specific")]
	[ReadOnlyField] public bool IsFireMode;
	[ReadOnlyField] public bool IsTransforming;
	[Header("Fire Warrior's Fire Mode")]
	[Tooltip("Health increase per successful attack")][Min(0)] public float LifeStealAmount;
	[Tooltip("Health decrease per LifeDrainTick")][Min(0)] public float LifeDrainAmount;
	[Tooltip("In seconds")][Min(0)] public float LifeDrainTick;
	[ReadOnlyField] public float LifeDrainTickLeft;
	[Min(0)] public float ExtraAttack;
	[Min(0)] public int ExtraJumpAmount;

	public override void ResetData()
	{ 
		base.ResetData();
		IsFireMode = false;
		IsTransforming = false;
		LifeDrainTickLeft = 0;
	}

	public override float GetAttack()
	{
		return IsFireMode ? Attack + ExtraAttack : Attack;
	}

	public float GetJumpAmount()
	{
		return IsFireMode ? JumpAmount + ExtraJumpAmount : JumpAmount;
	}
}
