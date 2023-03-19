using ICKT.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IRegisterable
{
	[SerializeField] private FireWarriorUnit _PlayerUnitPrefab;
	[SerializeField] private StoneMechaGolemUnit _EnemyUnitPrefab;
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
	}

	private void Start()
	{
		_PlayerUnit = Instantiate(_PlayerUnitPrefab);

		Vector3 enemyPosition = _PlayerUnit.transform.position + Vector3.left * 4f;
		_EnemyUnit = Instantiate(_EnemyUnitPrefab, enemyPosition, Quaternion.identity);
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
}
