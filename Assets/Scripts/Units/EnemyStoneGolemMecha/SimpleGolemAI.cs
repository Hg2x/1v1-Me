using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(StoneMechaGolemUnit))]
public class SimpleGolemAI : MonoBehaviour
{
	private StoneMechaGolemUnit _GolemUnit;
	[SerializeField] private StoneMechaGolemUnitData _Data;
	[SerializeField] private List<Vector2> _Waypoints;
	private int _CurrentWaypointIndex = 0;
	[SerializeField] private bool _IsCycleWaypoints = true;

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
		_GolemUnit.ChangeGolemAnimationState(StoneMechaGolemUnit.IDLE);
		MoveToNextWaypoint();
	}

	private void Update()
	{
		if (_Waypoints != null && _Waypoints.Count != 0)
		{
			if (Vector2.Distance(transform.position, _Waypoints[_CurrentWaypointIndex]) < 0.1f)
			{
				MoveToNextWaypoint();
				// TODO: add stay in waypoint duration
			}
		}
	}

	private void MoveToNextWaypoint()
	{
		if (_IsCycleWaypoints)
		{
			_CurrentWaypointIndex = (_CurrentWaypointIndex + 1) % _Waypoints.Count;
		}
		else
		{
			_CurrentWaypointIndex = ChooseRandomWaypointIndex();
		}

		Vector2 targetPosition = _Waypoints[_CurrentWaypointIndex];
		StartCoroutine(MoveToPosition(targetPosition, 3f));
	}

	private int ChooseRandomWaypointIndex()
	{
		return Random.Range(0, _Waypoints.Count);
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

	// ATTACK SEQUENCES

	// shoot arm
	// laser
	// shoot arm -> arm swing
	// go right next to player quickly -> arm swing

	// (shoot arm quickly -> update facing direction)x2
	// shoot arm -> arm swing -> laser
	// (shoot arm -> update facing direction)x3 -> arm swing
	// keep following player and spam arm swing

	// shoot arm 6x quickly while moving
	// laser 2x while moving
}
