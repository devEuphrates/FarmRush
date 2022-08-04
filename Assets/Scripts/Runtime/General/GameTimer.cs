using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Euphrates
{
    public static class GameTimer
    {
        static bool _isRunning = false;
        static List<Timer> _timers = new List<Timer>();

        public static void CreateTimer(string name, float duration, Action onFinish = null, Action<float> onTick = null, Action onCancle = null)
        {
            Timer timer = new Timer()
            {
                Name = name,
                Start = Time.time,
                End = Time.time + duration,
                OnTick = onTick,
                OnFinish = onFinish,
                OnCancle = onCancle
            };

            _timers.Add(timer);

            if (!_isRunning)
                RunTimers();
        }

        public static void CancleTimer(string name)
        {
            int indx = -1;
            for (int i = 0; i < _timers.Count; i++)
                if (_timers[i].Name == name)
                    indx = i;

            if (indx == -1)
                return;

            _timers[indx].OnCancle?.Invoke();
            _timers.RemoveAt(indx);
        }

        static async void RunTimers()
        {
            _isRunning = true;
            float last = Time.time;

            while (_timers.Count > 0)
            {
                for (int i = _timers.Count - 1; i > -1; i--)
                {
                    if (_timers[i].End < Time.time)
                    {
                        _timers[i].OnFinish?.Invoke();
                        _timers.RemoveAt(i);
                        continue;
                    }

                    _timers[i].OnTick?.Invoke(Time.time - last);
                }
                last = Time.time;
                await Task.Yield();
            }
            _isRunning = false;
        }
    }

    struct Timer
    {
        public string Name;
        public float Start;
        public float End;
        public float Duration => End - Start;
        public Action<float> OnTick;
        public Action OnFinish;
        public Action OnCancle;
    }
}
