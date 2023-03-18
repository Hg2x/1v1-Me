using ICKT.Services;
using ICKT.UI;
using UnityEngine;

public class UIBattle : UIBase
{
    [SerializeField] UIBattleTimer _Timer;

	private void Start()
	{
		ServiceLocator.Get<LevelManager>().OnSecondPassed += UIBattle_OnSecondPassed;
	}

	private void UIBattle_OnSecondPassed(float totalTime)
	{
		_Timer.IncreaseTimerText(totalTime);
	}

	private void OnDestroy()
	{
		if (ServiceLocator.IsRegistered<LevelManager>())
		{
			ServiceLocator.Get<LevelManager>().OnSecondPassed -= UIBattle_OnSecondPassed;
		}
	}
}
