using ICKT.Services;
using ICKT.UI;
using UnityEngine;

public class UIBattle : UIBase
{
    [SerializeField] private UIBattleTimer _TimerUI;
	[SerializeField] private UIBattleUnitInfo _PlayerUnitInfoUI;

	// TODO: use a better way to get unit infos
	[SerializeField] private UnitDataBase _PlayerData;

	private void Start()
	{
		ServiceLocator.Get<LevelManager>().OnSecondPassed += UIBattle_OnSecondPassed;
		_PlayerUnitInfoUI.Initialize(_PlayerData);
	}

	private void UIBattle_OnSecondPassed(float totalTime)
	{
		_TimerUI.IncreaseTimerText(totalTime);
	}

	private void OnDestroy()
	{
		if (ServiceLocator.IsRegistered<LevelManager>())
		{
			ServiceLocator.Get<LevelManager>().OnSecondPassed -= UIBattle_OnSecondPassed;
		}
	}
}
