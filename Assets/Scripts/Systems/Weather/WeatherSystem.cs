using UnityEngine;

namespace Lingtianlu
{
    public enum WeatherType
    {
        Sunny,
        Rain,
        Drought,
        Snow
    }

    public class WeatherSystem : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float rainChance = 0.2f;
        [SerializeField, Range(0f, 1f)] private float droughtChance = 0.08f;

        public WeatherType CurrentWeather { get; private set; } = WeatherType.Sunny;

        public void GenerateDailyWeather(int day)
        {
            float roll = Random.value;
            if (roll < droughtChance)
            {
                CurrentWeather = WeatherType.Drought;
                return;
            }

            if (roll < droughtChance + rainChance)
            {
                CurrentWeather = WeatherType.Rain;
                return;
            }

            CurrentWeather = day % 112 > 84 ? WeatherType.Snow : WeatherType.Sunny;
        }

        public float GetGrowthModifier()
        {
            return CurrentWeather switch
            {
                WeatherType.Rain => 1.2f,
                WeatherType.Drought => 0.65f,
                WeatherType.Snow => 0.5f,
                _ => 1f
            };
        }
    }
}
