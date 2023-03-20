using ICKT;
using ICKT.Audio;
using ICKT.Services;
using ICKT.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : UIBase
{
	[SerializeField] private AudioManagerData _AudioData;
	[SerializeField] private Slider _MasterVolumeSlider;
	[SerializeField] private Slider _MusicVolumeSlider;
	[SerializeField] private Slider _SfxVolumeSlider;

	private void OnEnable()
	{
		_MasterVolumeSlider.value = _AudioData.MasterVolume;
		_MusicVolumeSlider.value = _AudioData.MusicVolume;
		_SfxVolumeSlider.value = _AudioData.SfxVolume;
	}

	public override void Show()
	{
		base.Show();
		GameInstance.PauseGame();
	}

	public void OnVolumeSliderValueChanged()
	{
		_AudioData.MasterVolume = _MasterVolumeSlider.value;
		_AudioData.MusicVolume = _MusicVolumeSlider.value;
		_AudioData.SfxVolume = _SfxVolumeSlider.value;
		ServiceLocator.Get<AudioManager>().SetAllVolume();
	}

	public void OnResumeButtonClicked()
	{
		GameInstance.ResumeGame();
		Close();
	}

	public void OnRetryButtonClicked()
	{
		GameInstance.RetryStage();
	}
}
