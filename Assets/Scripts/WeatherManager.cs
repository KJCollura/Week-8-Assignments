using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [Header("Lighting Settings")]
    public Light sceneLight; // Assign your directional light (sun)
    public Color clearWeatherColor = Color.white;
    public Color rainyWeatherColor = new Color(0.5f, 0.5f, 0.6f);
    public float transitionSpeed = 2f;

    [Header("Rain Settings")]
    public ParticleSystem rainEffect; // Assign a particle system for rain
    private bool isRaining = false;

    private void Start()
    {
        if (sceneLight == null)
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    sceneLight = light;
                    break; // Stop searching after finding the first directional light
                }
            }

            if (sceneLight == null)
            {
                Debug.LogWarning("No Directional Light found! Assign one manually in the Inspector.");
            }
        }
        
        if (rainEffect != null)
        {
            rainEffect.Stop();
        }
        else
        {
            Debug.LogWarning("RainEffect NOT assigned in WeatherManager!");
        }
    }

    private void Update()
    {
        UpdateLighting();
    }

    private void UpdateLighting()
    {
        if (sceneLight == null)
        {
            Debug.LogWarning("Scene Light is null! Make sure it is assigned.");
            return;
        }
        
        Color targetColor = isRaining ? rainyWeatherColor : clearWeatherColor;
        sceneLight.color = Color.Lerp(sceneLight.color, targetColor, Time.deltaTime * transitionSpeed);
    }

    public void SetWeather(bool raining)
    {
        isRaining = raining;
        Debug.Log("Weather changed. Raining: " + raining);

        if (rainEffect != null)
        {
            if (raining)
            {
                Debug.Log("Playing rain effect.");
                rainEffect.gameObject.SetActive(false);
                rainEffect.gameObject.SetActive(true); // Force restart
                rainEffect.Play();
            }
            else
            {
                Debug.Log("Stopping rain effect.");
                rainEffect.Stop();
            }
        }
        else
        {
            Debug.LogWarning("RainEffect NOT assigned in WeatherManager!");
        }
    }
}
