using ICKT;
using ICKT.Audio;
using ICKT.Services;
using ICKT.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : UIBase
{
	[SerializeField] private AudioManagerData _AudioData;
	[SerializeField] private Button _ExitButton;
	[SerializeField] private Slider _MasterVolumeSlider;
	[SerializeField] private Slider _MusicVolumeSlider;
	[SerializeField] private Slider _SfxVolumeSlider;
	private bool _IsInitialized = false;

	private void Awake()
	{
		_MasterVolumeSlider.value = _AudioData.MasterVolume;
		_MusicVolumeSlider.value = _AudioData.MusicVolume;
		_SfxVolumeSlider.value = _AudioData.SfxVolume;

		if (GameInstance.IsWebGLBuild())
		{
			_MasterVolumeSlider.gameObject.SetActive(false);
			_MusicVolumeSlider.gameObject.SetActive(false);
			_SfxVolumeSlider.gameObject.SetActive(false);
			_ExitButton.gameObject.SetActive(false);
		}
		
		_IsInitialized = true;
	}

	public override void Show()
	{
		base.Show();
		GameInstance.PauseGame();
	}

	public void OnVolumeSliderValueChanged()
	{
		if (_IsInitialized)
		{
			_AudioData.MasterVolume = _MasterVolumeSlider.value;
			_AudioData.MusicVolume = _MusicVolumeSlider.value;
			_AudioData.SfxVolume = _SfxVolumeSlider.value;
			ServiceLocator.Get<AudioManager>().SetAllVolume();
		}
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

	public void OnExitButtonClicked()
	{
		GameInstance.ExitGame();
	}
}
