using UnityEngine;
using static ICKT.Editor.EditorLibrary;

[CreateAssetMenu(fileName = "FireWarriorUnitData", menuName = "ScriptableObject/UnitData/FireWarriorUnitData")]
public class FireWarriorUnitData : PlayerUnitData
{
	[Header("Fire Warrior Specific")]
	[ReadOnlyField] public bool _IsFireMode;
	[ReadOnlyField] public bool _IsTransforming;

	public override void ResetData()
	{ 
		base.ResetData();
		_IsFireMode = false;
		_IsTransforming = false;
	}
}
