using ICKT;
using ICKT.UI;

public class UIPause : UIBase
{
	public override void Show()
	{
		base.Show();
		GameInstance.PauseGame();
	}

	public void OnResumeButtonClicked()
	{
		GameInstance.ResumeGame();
		Close();
	}
}
