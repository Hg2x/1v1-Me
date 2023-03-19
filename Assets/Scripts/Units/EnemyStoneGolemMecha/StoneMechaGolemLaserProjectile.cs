using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMechaGolemLaserProjectile : DamageCollider
{
	[SerializeField] private BoxCollider2D _Collider;
	private Animator _Animator;
	private float _LaserDuration;

	private const string CHARGE_LASER = "ChargeLaser";
	private const string SHOOT_LASER = "ShootLaser";
	private const string LASER_DURATION = "LaserDuration";

	private void Awake()
	{
		_Animator = GetComponent<Animator>();
		_LaserDuration = _Animator.GetFloat(LASER_DURATION);
	}

	private void OnEnable()
	{
		
	}

	public void OnShootLaser()
	{
		_Animator.Play(SHOOT_LASER);
		_Collider.gameObject.SetActive(true);
	}

	public void OnShootLaserEnd()
	{
		_Collider.gameObject.SetActive(false);
		gameObject.SetActive(false);
	}

	public void StartCharge(float chargeSpeedMulitplier = 1f, float laserDuration = 0)
	{
		if (chargeSpeedMulitplier <= 0)
		{
			chargeSpeedMulitplier = 1f;
		}

		_LaserDuration = laserDuration;
		if (_LaserDuration <= 0)
		{
			_LaserDuration = 1.5f;
		}

		gameObject.SetActive(true);
		_Animator.Play(CHARGE_LASER);
		_Animator.SetFloat("PlaybackSpeed", chargeSpeedMulitplier);
	}
}
