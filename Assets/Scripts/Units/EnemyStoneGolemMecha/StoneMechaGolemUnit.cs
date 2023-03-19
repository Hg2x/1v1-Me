using ICKT.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StoneMechaGolemUnit : UnitBase
{
	private StoneMechaGolemUnitData _Data;
	[SerializeField] private DamageCollider _LeftMeleeCollider;
	[SerializeField] private DamageCollider _RightMeleeCollider;
	[SerializeField] private StoneMechaGolemArmProjectile _ProjectilePrefab;
	[SerializeField] private StoneMechaGolemLaserProjectile _LaserPrefab;
	[SerializeField] private Transform _LaserSpawnPoint;
	private GolemProjectilePool _ProjectilePool;

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
		SetAttackDamage(_Data.GetAttack());
		_LeftMeleeCollider.SetParent(gameObject);
		_RightMeleeCollider.SetParent(gameObject);
		_LaserPrefab.SetParent(gameObject);
		_LaserPrefab.SetDamageAmount(_Data.GetAttack());
		if (gameObject.TryGetComponent(out GolemProjectilePool pool))
		{
			_ProjectilePool = pool;
			_ProjectilePool.Initialize(_Data, _ProjectilePrefab);
		}
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

			// TODO: make combine animations to make reusable attack patterns
			if (_CurrentAnimation == IDLE)
			{
				ChangeAnimationState(SHOOT_ARM);
			}
			else if (_CurrentAnimation == SHOOT_ARM)
			{
				ChangeAnimationState(ATTACK);
			}
			else if (_CurrentAnimation == ATTACK)
			{
				ChangeAnimationState(SHOOT_LASER);
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

	public void OnShootArm()
	{
		var targetPosition = ServiceLocator.Get<LevelManager>().PlayerTransform.position;
		_ProjectilePool.LaunchNormalProjectile((Vector2)targetPosition); // Normal shoot
	}

	public void StartLaser()
	{
		// TODO: change to left/right direction
		_LaserPrefab.transform.position = _LaserSpawnPoint.transform.position;
		_LaserPrefab.StartCharge();
	}

	private void SetAttackDamage(float attack)
	{
		_LeftMeleeCollider.SetDamageAmount(attack);
		_RightMeleeCollider.SetDamageAmount(attack);
	}
}
