using ICKT.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IRegisterable
{
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
			Debug.Log("Total time: " + _TotalTime);
			_ElapsedTime -= 1;
			OnSecondPassed?.Invoke(_TotalTime);
		}
	}
}
