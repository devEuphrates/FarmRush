using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Euphrates
{
	public class SceneManager : Singleton<SceneManager>
	{
        bool _sceneLoaded = false;

        [SerializeField] GameObject _loadScreen;
        public event Action OnReady;

        void Start()
        {
            Application.targetFrameRate = 60;
            LoadGame();
        }

        async void LoadGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1, UnityEngine.SceneManagement.LoadSceneMode.Additive).completed += OnSceneLoad;

            await SpawnManager.Instance.Init();
            float tStart = Time.time;

            while (!_sceneLoaded && Time.time < tStart + 20f)
                await Task.Yield();

            OnReady?.Invoke();

            _loadScreen.SetActive(false);
        }

        void OnSceneLoad(AsyncOperation op) => _sceneLoaded = true;
    }
}
