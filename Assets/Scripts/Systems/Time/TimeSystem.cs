using System;
using UnityEngine;

namespace Lingtianlu
{
    public class TimeSystem : MonoBehaviour
    {
        [SerializeField, Min(60f)] private float realSecondsPerDay = 600f;

        public event Action<int> DayChanged;

        public int Day { get; private set; } = 1;
        public int Season { get; private set; } = 1;
        public float DayProgress { get; private set; }

        private float dayTimer;

        private void Update()
        {
            dayTimer += UnityEngine.Time.deltaTime;
            DayProgress = Mathf.Clamp01(dayTimer / realSecondsPerDay);

            if (dayTimer < realSecondsPerDay)
            {
                return;
            }

            AdvanceDay();
        }

        public void AdvanceDay()
        {
            dayTimer = 0f;
            Day++;

            if ((Day - 1) % 28 == 0)
            {
                Season = Mathf.Clamp(((Day - 1) / 28) % 4 + 1, 1, 4);
            }

            DayChanged?.Invoke(Day);
        }
    }
}
