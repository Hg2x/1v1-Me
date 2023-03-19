using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMechaGolemUnit : UnitBase
{
	private StoneMechaGolemUnitData _Data;

	private const string IDLE = "Idle";
	private const string ATTACK = "Attack";
	private const string SHOOT_ARM = "ShootArm";
	private const string SHOOT_LASER = "ShootLaser";

	private float _ElapsedTime = 0;
	private float _TotalTime = 0;

	protected override void Awake()
	{
		base.Awake();
		_Data = _DataBaseForm as StoneMechaGolemUnitData;
		if (_Data == null)
		{
			Debug.LogError("StoneMechaGolemUnitData initialize cast failed");
			return;
		}
		_Data.ResetData();
	}

	private void Start()
	{
		ChangeAnimationState(IDLE);
	}

	private void Update()
	{
		_ElapsedTime += Time.deltaTime;
		_TotalTime += Time.deltaTime;

		if (_ElapsedTime >= 1)
		{
			_ElapsedTime -= 1;
			if (_CurrentAnimation == IDLE)
			{
				ChangeAnimationState(ATTACK);
			}
			else
			{
				ChangeAnimationState(IDLE);
			}
		}
	}
}
