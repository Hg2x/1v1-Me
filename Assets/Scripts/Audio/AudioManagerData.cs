using FMODUnity;
using UnityEngine;

namespace ICKT.Audio
{
	[CreateAssetMenu(fileName = "AudioManagerData", menuName = "Audio/ManagerData/AudioManagerData")]
	public class AudioManagerData : ScriptableObject
	{
		[Header("Audio Volume")]
		[Range(0, 1)]
		public float MasterVolume;
		[Range(0, 1)]
		public float MusicVolume;
		[Range(0, 1)]
		public float SfxVolume;


		// Audio event references below, will need to be migrated elsewhere if projects gets too big

		[field: Header("Music")]
		[field: SerializeField] public EventReference StartingMusic { get; private set; }
	}
}
