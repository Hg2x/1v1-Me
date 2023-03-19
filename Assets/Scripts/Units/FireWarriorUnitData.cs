using UnityEngine;
using static ICKT.Editor.EditorLibrary;

[CreateAssetMenu(fileName = "FireWarriorUnitData", menuName = "ScriptableObject/PlayerUnitData/FireWarriorUnitData")]
public class FireWarriorUnitData : PlayerUnitData
{
	[Header("Fire Warrior Specific")]
	[ReadOnlyField] public bool _IsFireMode;
	[ReadOnlyField] public bool _IsTransforming;
	[Header("Fire Warrior's Fire Mode")]
	[Tooltip("Health increase per successful attack")][Min(0)] public float LifeStealAmount;
	[Tooltip("Health decrease per LifeDrainTick")][Min(0)] public float LifeDrainAmount;
	[Tooltip("In seconds")][Min(0)] public float LifeDrainTick;
	[Min(0)] public float ExtraAttack;
	[Min(0)] public int ExtraJumpAmount;

	public override void ResetData()
	{ 
		base.ResetData();
		_IsFireMode = false;
		_IsTransforming = false;
	}

	public float GetAttack()
	{
		return _IsFireMode ? Attack + ExtraAttack : Attack;
	}

	public float GetJumpAmount()
	{
		return _IsFireMode ? JumpAmount + ExtraJumpAmount : JumpAmount;
	}
}
