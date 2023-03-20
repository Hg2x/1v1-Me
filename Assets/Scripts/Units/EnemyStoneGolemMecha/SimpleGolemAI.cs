using ICKT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GolemAttackType
{
	ArmSwing,
	ShootArm,
	ShootLaser
}

[RequireComponent(typeof(StoneMechaGolemUnit))]
public class SimpleGolemAI : MonoBehaviour
{
	private StoneMechaGolemUnit _GolemUnit;
	[SerializeField] private StoneMechaGolemUnitData _Data;
	[SerializeField] private List<Vector2> _Waypoints;
	private int _CurrentWaypointIndex = 0;
	private bool _IsCycleWaypoints = true;
	private readonly float _WaitTimeBeforeNextAction = 4f;
	private float _WaitTimeLeft = 0;
	private int _WaypointCycleLeft = 0;

	//TODO: remove all hardcoded values

	private void Awake()
	{
		_GolemUnit = GetComponent<StoneMechaGolemUnit>();
	}

	public void SetWaypoints(List<Vector2> waypoints)
	{
		_Waypoints = waypoints;
	}

	private void Start()
	{
		ToIdle();
		MoveToNextWaypoint();
	}

	private void Update()
	{
		if (_Waypoints != null && _Waypoints.Count != 0)
		{
			if (Vector2.Distance(transform.position, _Waypoints[_CurrentWaypointIndex]) < 0.1f)
			{
				if (_WaitTimeLeft > 0)
				{
					_WaitTimeLeft -= Time.deltaTime;
				}
				else
				{
					_WaitTimeLeft = _WaitTimeBeforeNextAction;
					MoveToNextWaypoint();
					DoRandomAttackSequence();
				}
				// TODO: better way to tweak stay in waypoint duration and behaviour
			}
		}
	}

	private void MoveToNextWaypoint()
	{
		if (_WaypointCycleLeft <= 0)
		{
			_IsCycleWaypoints = FunctionLibrary.GetRandomBool();
			_WaypointCycleLeft = FunctionLibrary.GetRandomNumber(1, _Waypoints.Count - 1);
		}

		if (_IsCycleWaypoints)
		{
			_CurrentWaypointIndex = (_CurrentWaypointIndex + 1) % _Waypoints.Count;
		}
		else
		{
			_CurrentWaypointIndex = ChooseRandomWaypointIndex();
		}
		_WaypointCycleLeft--;

		Vector2 targetPosition = _Waypoints[_CurrentWaypointIndex];
		StartCoroutine(MoveToPosition(targetPosition, 3f));
	}

	private int ChooseRandomWaypointIndex()
	{
		return UnityEngine.Random.Range(0, _Waypoints.Count);
	}

	private IEnumerator MoveToPosition(Vector2 targetPosition, float duration)
	{
		Vector2 startPosition = transform.position;
		float elapsedTime = 0;

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			float progress = elapsedTime / duration;
			transform.position = Vector2.Lerp(startPosition, targetPosition, progress);
			yield return null;
		}
	}

	private void ToIdle()
	{
		_GolemUnit.ChangeGolemAnimationState(StoneMechaGolemUnit.IDLE);
	}

	private void SingleAttack(GolemAttackType attackType, Action onAttackDoneCallback = null)
	{
		string attackAnimation = attackType switch
		{
			GolemAttackType.ArmSwing => StoneMechaGolemUnit.ATTACK,
			GolemAttackType.ShootArm => StoneMechaGolemUnit.SHOOT_ARM,
			GolemAttackType.ShootLaser => StoneMechaGolemUnit.SHOOT_LASER,
			_ => throw new ArgumentOutOfRangeException(nameof(attackType), attackType, "Invalid GolemAttackType")
		};

		_GolemUnit.FaceTowardsPlayer();
		onAttackDoneCallback ??= ToIdle;
		StartCoroutine(_GolemUnit.ChangeGolemAnimationAndWait(attackAnimation, onAttackDoneCallback));
	}

	private IEnumerator AttackWithDelay(GolemAttackType attackType, float delay, Action onAttackDoneCallback = null)
	{
		SingleAttack(attackType);
		yield return new WaitForSeconds(delay);
		onAttackDoneCallback?.Invoke();
	}

	private void DoRandomAttackSequence()
	{
		List<Action> attackList = new()
		{
			SwingArmOne,
			ShootArmOne,
			ShootLaserOne,
			ShootArmSwingOne,
			ShootLaserArmOne,
			ShootArmLaserSwingOne
		};

		int index = FunctionLibrary.GetRandomNumber(0, attackList.Count - 1);
		attackList[index]?.Invoke();
	}


	// ATTACK SEQUENCES

	// swing arm
	private void SwingArmOne() => SingleAttack(GolemAttackType.ArmSwing);

	// shoot arm
	private void ShootArmOne() => SingleAttack(GolemAttackType.ShootArm);

	// laser
	private void ShootLaserOne() => SingleAttack(GolemAttackType.ShootLaser);

	// shoot arm -> arm swing
	private void ShootArmSwingOne()
	{
		SingleAttack(GolemAttackType.ShootArm, SwingArmOne);
	}

	// shoot laser -> shoot arm
	private void ShootLaserArmOne()
	{
		SingleAttack(GolemAttackType.ShootLaser, ShootArmOne);
	}

	// go right next to player quickly -> arm swing

	// (shoot arm quickly -> update facing direction)x2

	// shoot arm -> arm swing -> laser
	private void ShootArmLaserSwingOne()
	{
		void shootLaserThenArmSwing() => SingleAttack(GolemAttackType.ShootLaser, SwingArmOne);
		SingleAttack(GolemAttackType.ShootArm, shootLaserThenArmSwing);
	}

	// (shoot arm -> update facing direction)x3 -> arm swing
	// keep following player and spam arm swing

	// shoot arm 6x quickly while moving
	// laser 2x while moving
}
