using ICKT;
using UnityEngine;

public class StoneMechaGolemUnit : UnitBase
{
	private StoneMechaGolemUnitData _Data;
	[SerializeField] private DamageCollider _LeftMeleeCollider;
	[SerializeField] private DamageCollider _RightMeleeCollider;
	[SerializeField] private StoneMechaGolemArmProjectile _ProjectilePrefab;
	[SerializeField] private StoneMechaGolemLaserProjectile _LaserPrefab;
	[SerializeField] private Transform _LeftLaserSpawnPoint;
	[SerializeField] private Transform _RightLaserSpawnPoint;
	private GolemProjectilePool _ProjectilePool;

	public const string IDLE = "Idle";
	public const string ATTACK = "Attack";
	public const string SHOOT_ARM = "ShootArm";
	public const string SHOOT_LASER = "ShootLaser";

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

	private void Update()
	{
		_ElapsedTime += Time.deltaTime;
		_TotalTime += Time.deltaTime;

		FaceTowardsPlayer();

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
		if (_Data.IsFacingRight)
		{
			_RightMeleeCollider.gameObject.SetActive(true);
		}
		else
		{
			_LeftMeleeCollider.gameObject.SetActive(true);
		}
	}

	public void OnArmSwingEnd()
	{
		_LeftMeleeCollider.gameObject.SetActive(false);
		_RightMeleeCollider.gameObject.SetActive(false);
	}

	public void OnShootArm()
	{
		_ProjectilePool.LaunchNormalProjectile(); // Normal shoot that targets player unit
	}

	public void StartLaser()
	{
		if (_Data.IsFacingRight)
		{
			_LaserPrefab.transform.position = _RightLaserSpawnPoint.position;
		}
		else
		{
			_LaserPrefab.transform.position = _LeftLaserSpawnPoint.position;
		}
		
		_LaserPrefab.SetDamageAmount(_Data.GetAttack());
		_LaserPrefab.StartCharge();
	}

	public void ChangeGolemAnimationState(string newAnimation)
	{
		ChangeAnimationState(newAnimation);
	}

	public void FaceTowardsPlayer()
	{
		if (_CurrentAnimation != IDLE) // use a better way to check if should flip
		{
			return;
		}

		float zRotationDegreesToPlayer = FunctionLibrary.GetRotationToPlayer2D((Vector2)transform.position).eulerAngles.z;
		if (zRotationDegreesToPlayer > 90 && zRotationDegreesToPlayer < 270)
		{
			_Data.IsFacingRight = false;
		}
		else
		{
			_Data.IsFacingRight = true;
		}

		FlipSprite(!_Data.IsFacingRight);
		
	}

	private void SetAttackDamage(float attack)
	{
		_LeftMeleeCollider.SetDamageAmount(attack);
		_RightMeleeCollider.SetDamageAmount(attack);
	}
}
