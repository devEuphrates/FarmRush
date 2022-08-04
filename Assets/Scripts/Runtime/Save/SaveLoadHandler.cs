using UnityEngine;

namespace Euphrates
{
    public class SaveLoadHandler : MonoBehaviour
    {
        [SerializeField] SaveLoadManagerSO _slManager;
        bool _firstLoad = false;

        private void Start() => SceneManager.Instance.OnReady += Init;

        void OnApplicationPause(bool pause)
        {
            if (pause)
                TrySave();
        }

        void OnApplicationFocus(bool focus)
        {
            if (!focus)
                TrySave();
        }

        void Init()
        {
            _slManager.Load();
            _firstLoad = true;
            GameTimer.CreateTimer("SaveGame", 10f, TimerSave);
        }

        void TimerSave()
        {
            Debug.Log("GAME SAVED");
            _slManager.Save();
            GameTimer.CreateTimer("SaveGame", 10f, TimerSave);
        }

        void TrySave()
        {
            if (!_firstLoad)
                return;

            _slManager.Save();
        }
    }
}
