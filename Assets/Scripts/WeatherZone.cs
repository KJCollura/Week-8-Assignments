using UnityEngine;

public class WeatherZone : MonoBehaviour
{
    public bool isRainyZone; // Set in Inspector: true = rain, false = clear

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WeatherManager weatherManager = FindObjectOfType<WeatherManager>();
            if (weatherManager != null)
            {
                weatherManager.SetWeather(isRainyZone);
            }
        }
    }
}
