using UnityEngine;
using System.Collections;
using System.Linq;

public class WeatherManager : MonoBehaviour
{
    [Header("Lighting Settings")]
    public Light sceneLight;
    public Color clearWeatherColor = Color.white;
    public Color rainyWeatherColor = new Color(0.5f, 0.5f, 0.6f);
    public float transitionSpeed = 2f;

    [Header("Rain Settings")]
    public ParticleSystem rainEffect;
    public AudioClip rainSoundClip;
    private Coroutine rainTransition;
    private bool isRaining = false;
    private float nextWeatherChangeTime;

    [Header("Thunderstorm Settings")]
    public AudioClip thunderSoundClip;
    public Light lightningFlash;

    private AudioSource audioSource; // Single AudioSource for playing sounds

    public enum WeatherType { Clear, LightRain, HeavyRain, Storm, Snow }
    public WeatherType currentWeather;

    private void Start()
    {
        nextWeatherChangeTime = Time.time + Random.Range(10, 30);

        audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource dynamically

        if (sceneLight == null)
        {
            sceneLight = FindFirstObjectByType<Light>();
            if (sceneLight == null || sceneLight.type != LightType.Directional)
            {
                Debug.LogWarning("No Directional Light found! Assign one manually in the Inspector.");
                sceneLight = null;
            }
        }

        if (rainEffect != null)
        {
            rainEffect.Stop();
        }
        else
        {
            Debug.LogWarning("RainEffect NOT assigned! Assign in the Inspector.");
        }
    }

    private void Update()
    {
        UpdateLighting();

        if (Time.time >= nextWeatherChangeTime)
        {
            ToggleWeather();
            nextWeatherChangeTime = Time.time + Random.Range(10, 30);
        }
    }

    private void UpdateLighting()
    {
        if (sceneLight == null) return;

        Color targetColor = isRaining ? rainyWeatherColor : clearWeatherColor;
        sceneLight.color = Color.Lerp(sceneLight.color, targetColor, Time.deltaTime * transitionSpeed);
    }

    public void SetWeather(WeatherType newWeather)
    {
        currentWeather = newWeather;

        if (rainTransition != null)
        {
            StopCoroutine(rainTransition);
        }
        rainTransition = StartCoroutine(TransitionRainEffect(newWeather));
    }

    private IEnumerator TransitionRainEffect(WeatherType newWeather)
    {
        if (rainEffect == null)
        {
            Debug.LogWarning("RainEffect is missing! Assign it in the Inspector.");
            yield break;
        }

        isRaining = (newWeather != WeatherType.Clear && newWeather != WeatherType.Snow);
        float targetEmissionRate = newWeather == WeatherType.LightRain ? 200 :
                                   newWeather == WeatherType.HeavyRain ? 1000 :
                                   newWeather == WeatherType.Storm ? 1500 : 0;

        ParticleSystem.EmissionModule emission = rainEffect.emission;
        float duration = 2f;
        float elapsedTime = 0f;
        float startRate = emission.rateOverTime.constant;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newRate = Mathf.Lerp(startRate, targetEmissionRate, elapsedTime / duration);
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(newRate);
            yield return null;
        }

        emission.rateOverTime = new ParticleSystem.MinMaxCurve(targetEmissionRate);

        if (!isRaining)
        {
            rainEffect.Stop();
            StopSound();
        }
        else
        {
            PlaySound(rainSoundClip, true); // Loop rain sound
        }

        if (newWeather == WeatherType.Storm)
        {
            StartCoroutine(ThunderEffect());
        }
    }

    private IEnumerator ThunderEffect()
    {
        if (lightningFlash == null || thunderSoundClip == null)
        {
            Debug.LogWarning("Thunder or Lightning effect missing! Assign them in the Inspector.");
            yield break;
        }

        while (isRaining && currentWeather == WeatherType.Storm)
        {
            yield return new WaitForSeconds(Random.Range(5, 15));
            if (!isRaining) break;

            lightningFlash.enabled = true;
            PlaySound(thunderSoundClip, false);
            yield return new WaitForSeconds(0.2f);
            lightningFlash.enabled = false;
        }
    }

    private void ToggleWeather()
    {
        WeatherType[] weatherOptions = { WeatherType.Clear, WeatherType.LightRain, WeatherType.HeavyRain, WeatherType.Storm };
        SetWeather(weatherOptions[Random.Range(0, weatherOptions.Length)]);
    }

    private void PlaySound(AudioClip clip, bool loop)
    {
        if (audioSource == null || clip == null) return;
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    private void StopSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
