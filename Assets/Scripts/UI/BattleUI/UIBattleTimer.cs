using TMPro;
using UnityEngine;

public class UIBattleTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _MinutesText;
    [SerializeField] TextMeshProUGUI _SecondsText;

    public void IncreaseTimerText(float totalTime)
    {
        if (totalTime < 0)
        {
            totalTime = 0;
		}

		int minutes = Mathf.FloorToInt(totalTime / 60f);
		int seconds = Mathf.FloorToInt(totalTime % 60f);
		SetMinutesText(minutes);
		SetSecondstext(seconds);
	}

    private void SetMinutesText(int minutes)
    {
		_MinutesText.text = minutes.ToString();
	}

    private void SetSecondstext(int seconds)
    {
        if (seconds >= 10)
        {
			_SecondsText.text = seconds.ToString();
            return;
		}

		_SecondsText.text = "0" + seconds.ToString();
	}
}
