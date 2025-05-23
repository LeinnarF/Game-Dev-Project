using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.SceneManagement;

public class LightIntensityController : MonoBehaviour
{
    public static LightIntensityController Instance { get; private set; }

    public Light2D lightSource;
    public Light2D NightSource;
    public Light2D LampLight;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dayText;

    [Header("Main Time Setting")]
    public int realMinutesPerGameDay = 24;

    private float currentTime = 6f;
    private int currentDay = 1;
    private float timeSpeed;

    private float maxIntensity = 1f;
    private float minIntensity = 0f;
    private float noonIntensity = 1.5f;
    private float midnightIntensity = 0.5f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Sleep()
    {
        currentTime = 6f;
        currentDay++;
        lightSource.intensity = maxIntensity;
        NightSource.intensity = minIntensity;
        LampLight.intensity = minIntensity;
        UpdateGameTime();
        Debug.Log("Player slept. A new day has begun!");
    }

    void Start()
    {
        if (lightSource == null)
            lightSource = GetComponent<Light2D>();

        float totalRealSeconds = realMinutesPerGameDay * 60f;
        timeSpeed = 1440f / totalRealSeconds; // 1440 in-game minutes per day
    }

    void Update()
    {
        UpdateGameTime();
        UpdateLighting();
    }

    void UpdateGameTime()
    {
        currentTime += Time.deltaTime * timeSpeed / 60f;

        if (currentTime >= 24f)
        {
            currentTime -= 24f;
            currentDay++;
        }

        int hours = Mathf.FloorToInt(currentTime);
        int minutes = Mathf.FloorToInt((currentTime - hours) * 60f);

        string ampm = (hours >= 12) ? "PM" : "AM";
        int hour12 = hours % 12;
        if (hour12 == 0) hour12 = 12;

        if (timeText != null)
            timeText.text = $"{hour12:00}:{minutes:00} {ampm}";

        if (dayText != null)
            dayText.text = $"Day {currentDay}";
    }

    void UpdateLighting()
    {
        float t = currentTime;

        // Sunrise: 4:40 - 6:30 AM
        if (t >= 4.5f && t < 6.5f)
        {
            float progress = Mathf.InverseLerp(4.5f, 6.5f, t);
            lightSource.intensity = Mathf.Lerp(minIntensity, maxIntensity, progress);
            NightSource.intensity = Mathf.Lerp(midnightIntensity, minIntensity, progress);
            LampLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, progress);
        }
        // Morning: 6:00 - 12:00 PM (intensity at noon)
        else if (t >= 6.5f && t < 12f)
        {
            float progress = Mathf.InverseLerp(6.5f, 12f, t);
            lightSource.intensity = Mathf.Lerp(maxIntensity, noonIntensity, progress);
            NightSource.intensity = minIntensity;
            LampLight.intensity = minIntensity;
        }
        // Midday Hold: 12:00 - 4:00 PM
        else if (t >= 12f && t < 16f)
        {
            float progress = Mathf.InverseLerp(12f, 16f, t);
            lightSource.intensity = Mathf.Lerp(noonIntensity, maxIntensity, progress);
            NightSource.intensity = minIntensity;
            LampLight.intensity = minIntensity;
        }
        // Sunset: 2:00 PM - 8:00 PM
        else if (t >= 16f && t < 20f)
        {
            float progress = Mathf.InverseLerp(16f, 20f, t);
            NightSource.intensity = Mathf.Lerp(minIntensity, midnightIntensity, progress);
            lightSource.intensity = Mathf.Lerp(maxIntensity, minIntensity, progress);
            LampLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, progress);
        }
        // Night: 8:00 PM - 4:30 AM
        else
        {
            lightSource.intensity = minIntensity;
            NightSource.intensity = midnightIntensity;
            LampLight.intensity = maxIntensity;
        }
        
    }
}
// This script controls the light intensity based on the time of day in a game.