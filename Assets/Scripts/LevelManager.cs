using ICKT.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IRegisterable
{
	[SerializeField] private FireWarriorUnit _PlayerUnitPrefab;
	[SerializeField] private StoneMechaGolemUnit _EnemyUnitPrefab;
	[SerializeField] private Transform _PlayerSpawnPoint; // TODO: change fetching spawn point implementation
	[SerializeField] private Transform _EnemySpawnPoint;
	[SerializeField] private GameObject _WaypointsGameObject; // TODO: change fetching waypoints implementation
	private List<Vector2> _Waypoints;
	private FireWarriorUnit _PlayerUnit;
	public Transform PlayerTransform
	{
		get { return _PlayerUnit ? _PlayerUnit.transform : null; }
	}
	private StoneMechaGolemUnit _EnemyUnit;

	private float _ElapsedTime = 0;
	private float _TotalTime = 0;

	public delegate void SecondPassedHandler(float totalTime);
	public event SecondPassedHandler OnSecondPassed;

	public bool IsPersistent() => false;

	private void Awake()
	{
		ServiceLocator.Register(this);
		InitializeWaypoints();
	}

	private void Start()
	{
		_PlayerUnit = Instantiate(_PlayerUnitPrefab, _PlayerSpawnPoint);
		_EnemyUnit = Instantiate(_EnemyUnitPrefab, _EnemySpawnPoint);
		if (_EnemyUnit.gameObject.TryGetComponent(out SimpleGolemAI simpleAI)) // TODO: change this implementation
		{
			simpleAI.SetWaypoints(_Waypoints);
		}

	}

	private void OnDestroy()
	{
		ServiceLocator.Unregister(this);
		OnSecondPassed = null;
	}

	private void Update()
    {
		// TODO: separate timer to its own class if there's time
		_ElapsedTime += Time.deltaTime;
		_TotalTime += Time.deltaTime;

		if (_ElapsedTime >= 1)
		{
			_ElapsedTime -= 1;
			OnSecondPassed?.Invoke(_TotalTime);
		}
	}

	private void InitializeWaypoints()
	{
		if (_WaypointsGameObject == null)
		{
			Debug.LogError("Waypoints gameObject in LevelManager is null");
			return;
		}

		_Waypoints = new List<Vector2>();
		int childCount = _WaypointsGameObject.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = _WaypointsGameObject.transform.GetChild(i);
			_Waypoints.Add((Vector2)child.position);
		}
	}
}
