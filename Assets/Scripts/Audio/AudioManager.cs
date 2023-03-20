using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using ICKT.Services;

namespace ICKT.Audio
{
	[AutoRegisteredService]
	public class AudioManager : MonoBehaviour, IRegisterable
	{
		public static AudioManager Instance { get; private set; }

		private AudioManagerData _Data;
		private bool _IsInitialized = false;

		private VCA _MasterVCA;
		private VCA _MusicVCA;
		private VCA _SfxVCA;

		private const string MasterVCApath = "vca:/Master";
		private const string MusicVCApath = "vca:/Music";
		private const string SfxVCApath = "vca:/Sfx";

		private List<EventInstance> _EventInstances = new();
		private List<StudioEventEmitter> _EventEmitters = new();
		private EventInstance _MusicEventInstance;

		public bool IsPersistent() => true;

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;

				_MasterVCA = RuntimeManager.GetVCA(MasterVCApath);
				_MusicVCA = RuntimeManager.GetVCA(MusicVCApath);
				_SfxVCA = RuntimeManager.GetVCA(SfxVCApath);
			}
			else
			{
				Debug.LogError("Found more than one Audio Manager in the scene.");
			}
		}

		public void Initialize(AudioManagerData data)
		{
			if (data == null)
			{
				return;
			}

			if (!_IsInitialized)
			{
				_Data = data;
				
				_IsInitialized = true;

				InitializeMusic(_Data.StartingMusic);
			}
			else
			{
				Debug.LogError("AudioManager is already initialized.");
			}
		}

		public bool IsPlaying(EventInstance instance)
		{
			instance.getPlaybackState(out PLAYBACK_STATE state);
			return state != PLAYBACK_STATE.STOPPED;
		}

		public void PlayOneShot(EventReference sound, Vector3 worldPos)
		{
			RuntimeManager.PlayOneShot(sound, worldPos);
		}

		public void PlayOneShot(string path, Vector3 worldPos)
		{
			RuntimeManager.PlayOneShot(path, worldPos);
		}

		#region Set Volume
		public void SetMasterVolume(float volume)
		{
			if (_IsInitialized)
			{
				_Data.MasterVolume = Mathf.Clamp01(volume);
				_MasterVCA.setVolume(_Data.MasterVolume);
			}
		}

		public void SetMusicVolume(float volume)
		{
			if (_IsInitialized)
			{
				_Data.MusicVolume = Mathf.Clamp01(volume);
				_MusicVCA.setVolume(_Data.MusicVolume);
			}
		}

		public void SetSfxVolume(float volume)
		{
			if (_IsInitialized)
			{
				_Data.SfxVolume = Mathf.Clamp01(volume);
				_SfxVCA.setVolume(_Data.SfxVolume);
			}
		}
		#endregion

		public EventInstance CreateInstance(EventReference eventReference)
		{
			EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
			_EventInstances.Add(eventInstance);
			return eventInstance;
		}

		public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
		{
			StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
			emitter.EventReference = eventReference;
			_EventEmitters.Add(emitter);
			return emitter;
		}

		// TODO: ways to change and stop multiple music files
		private void InitializeMusic(EventReference musicEventReference)
		{
			_MusicEventInstance = CreateInstance(musicEventReference);
			_MusicEventInstance.start();
		}

		private void CleanUp()
		{
			// stop and release any created instances
			foreach (EventInstance eventInstance in _EventInstances)
			{
				eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				eventInstance.release();
			}
			// stop all of the event emitters, because if we don't they may hang around in other scenes
			foreach (StudioEventEmitter emitter in _EventEmitters)
			{
				emitter.Stop();
			}
		}

		private void OnDestroy()
		{
			CleanUp();
		}
	}
}
