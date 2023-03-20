using ICKT;
using ICKT.Services;
using ICKT.UI;
using TMPro;
using UnityEngine;

public class UIGameOver : UIBase
{
	[SerializeField] private TextMeshProUGUI _TitleText;

	public override void Show()
	{
		base.Show();
		if (_TitleText != null)
		{
			if (ServiceLocator.Get<LevelManager>().PlayerWon)
			{
				_TitleText.text = "YOU WIN";
			}
			else
			{
				_TitleText.text = "YOU LOSE";
			}
		}
	}

	public void OnRetryButtonClicked()
	{
		GameInstance.RetryStage();
	}
}
