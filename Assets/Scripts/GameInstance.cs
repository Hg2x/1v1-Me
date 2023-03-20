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

		private void Awake()
		{
			if (_Instance != null)
			{
				Destroy(gameObject);
			}

			_Instance = this;
			_ServiceLocator = gameObject.AddComponent<ServiceLocator>();
			_ServiceLocator.Initialize();
			_UIManager = gameObject.AddComponent<UIManager>();
			_UIManager.Init(_UIManagerData);
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
