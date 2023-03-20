using ICKT;
using ICKT.Services;
using ICKT.UI;
using UnityEngine;

public class UIBattle : UIBase
{
    [SerializeField] private UIBattleTimer _TimerUI;
	[SerializeField] private UIBattleUnitInfo _PlayerUnitInfoUI;
	[SerializeField] private UIBattleUnitInfo _EnemyUnitInfoUI;

	// TODO: use a better way to get unit infos
	[SerializeField] private UnitDataBase _PlayerData;
	[SerializeField] private UnitDataBase _EnemyData;

	private void Start()
	{
		ServiceLocator.Get<LevelManager>().OnSecondPassed += UIBattle_OnSecondPassed;
		_PlayerUnitInfoUI.Initialize(_PlayerData);
		_EnemyUnitInfoUI.Initialize(_EnemyData);
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

	public void OnPauseButtonClicked()
	{
		UIManager.Show<UIPause>();
	}
}
