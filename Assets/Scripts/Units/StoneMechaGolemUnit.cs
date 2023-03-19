using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMechaGolemUnit : UnitBase
{
	private StoneMechaGolemUnitData _Data;

	protected override void Awake()
	{
		base.Awake();
		_Data = _InitData as StoneMechaGolemUnitData;
		if (_Data == null)
		{
			Debug.LogError("StoneMechaGolemUnitData initialize cast failed");
			return;
		}
		_Data.ResetData();
	}

	private void Start()
	{
		//ChangeAnimationState(IDLE);
	}
}
