using System.ComponentModel;
using UnityEngine;
using static ICKT.Editor.EditorLibrary;

[CreateAssetMenu(fileName = "StoneMechaGolemUnitData", menuName = "ScriptableObject/EnemyUnitData/StoneMechaGolemUnitData")]
public class StoneMechaGolemUnitData : EnemyUnitData
{
	[Min(0)] public float ProjectileSpeed = 5f;
	[ReadOnlyField] public bool IsGlowing = false;
	[Min(0)] public float ProjectileDuration = 3f;

	public override void ResetData()
	{
		base.ResetData();
		IsGlowing = false;
	}
}
