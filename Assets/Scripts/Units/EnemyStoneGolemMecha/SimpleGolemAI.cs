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

	private void Awake()
	{
		_GolemUnit = GetComponent<StoneMechaGolemUnit>();
		// TODO: get waypoints from level manager
	}

	private void Start()
	{
		_GolemUnit.ChangeGolemAnimationState(StoneMechaGolemUnit.IDLE);
		//MoveToNextWaypoint();
	}

	private void Update()
	{
		//if (Vector2.Distance(transform.position, _Waypoints[_CurrentWaypointIndex]) < 0.1f)
		//{
		//	MoveToNextWaypoint();
		//}
		StartCoroutine(MoveToPosition(new Vector2(5, -1), 2f));
	}

	private void MoveToNextWaypoint()
	{
		// TODO: choose waypoint index with bias
		Vector2 targetPosition = _Waypoints[_CurrentWaypointIndex];
		//StartCoroutine(MoveToPosition(targetPosition, 3f));
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
