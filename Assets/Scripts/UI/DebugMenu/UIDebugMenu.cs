using ICKT;
using ICKT.UI;
using UnityEngine;

public class UIDebugMenu : UIBase
{
	public override void Show()
    {
        base.Show();
		GameInstance.PauseGame();
	}

	public void OnCloseButtonClicked()
    {
        GameInstance.ResumeGame();
        Close();
    }
}
