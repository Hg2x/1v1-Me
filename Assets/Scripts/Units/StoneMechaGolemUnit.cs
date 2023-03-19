using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMechaGolemUnit : UnitBase
{
	private StoneMechaGolemUnitData _Data;
	[SerializeField] private MeleeCollider _LeftMeleeCollider;
	[SerializeField] private MeleeCollider _RightMeleeCollider;

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
		_LeftMeleeCollider.SetParent(gameObject);
		_RightMeleeCollider.SetParent(gameObject);
		SetAttackDamage(_Data.GetAttack());
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

	public void OnArmSwing()
	{
		_RightMeleeCollider.gameObject.SetActive(true);
	}

	public void OnArmSwingEnd()
	{
		_RightMeleeCollider.gameObject.SetActive(false);
	}

	private void SetAttackDamage(float attack)
	{
		_LeftMeleeCollider.SetDamageAmount(attack);
		_RightMeleeCollider.SetDamageAmount(attack);
	}
}
