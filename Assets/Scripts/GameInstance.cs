using ICKT.Audio;
using ICKT.Services;
using ICKT.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ICKT
{
	public class GameInstance : MonoBehaviour
	{
		private static GameInstance _Instance;
		private static ServiceLocator _ServiceLocator;
		private UIManager _UIManager;
		[SerializeField] private UIManagerData _UIManagerData;
		[SerializeField] private AudioManagerData _AudioManagerData;

		private void Awake()
		{
			if (_Instance != null)
			{
				Destroy(gameObject);
				return;
			}

			_Instance = this;
			_ServiceLocator = gameObject.AddComponent<ServiceLocator>();
			_ServiceLocator.Initialize();
			_UIManager = gameObject.AddComponent<UIManager>();
			_UIManager.Init(_UIManagerData);
			ServiceLocator.Get<AudioManager>().Initialize(_AudioManagerData);
			DontDestroyOnLoad(_Instance);
		}

		public static void RetryStage()
		{
			ResumeGame();
			UIManager.ClearAllUI();
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		private static void GoToScene(string SceneName, Action callback = null)
		{
			ResumeGame();
			UIManager.ClearAllUI();
			_Instance.StartCoroutine(LoadSceneAsync(SceneName, callback));
		}

		public static void PauseGame()
		{
			Time.timeScale = 0;
		}

		public static void ResumeGame()
		{
			Time.timeScale = 1;
		}

		public static void ExitGame()
		{
			Application.Quit();
		}

		public static bool IsWebGLBuild()
		{
			return Application.platform == RuntimePlatform.WebGLPlayer;
		}

		private static IEnumerator LoadSceneAsync(string sceneName, Action callback = null)
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
			while (!asyncLoad.isDone)
			{
				yield return null;
			}

			callback?.Invoke();
		}
	}
}
