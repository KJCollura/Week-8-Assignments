using UnityEngine;

public class WeatherZone : MonoBehaviour
{
    public WeatherManager.WeatherType weatherType; // Select weather for this zone

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WeatherManager weatherManager = FindFirstObjectByType<WeatherManager>();
            if (weatherManager != null)
            {
                weatherManager.SetWeather(weatherType);
                Debug.Log("Player entered weather zone: " + weatherType);
            }
            else
            {
                Debug.LogWarning("WeatherManager NOT found in scene!");
            }
        }
    }
}
