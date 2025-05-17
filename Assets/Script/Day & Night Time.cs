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

    // ✅ CONFIGURATION (change only this value)
    [Header("Main Time Setting")]
    public int realMinutesPerGameDay = 24; // << CHANGE THIS to control everything!

    // ✅ Auto-calculated (do not change manually)
    private float currentTime = 6f;
    private int currentDay = 1;
    private float timeSpeed;
    private float intensityChangeRate;
    private float DelayTime;

    private float targetIntensity = 1f;
    private float minIntensity = 0f;
    private bool increasing = true;
    private bool isPaused = false;

    public void Awake()
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

    void Start()
    {
        if (lightSource == null)
            lightSource = GetComponent<Light2D>();

        // ✅ Auto-calculate values
        float totalRealSeconds = realMinutesPerGameDay * 60f;
        timeSpeed = 1440f / totalRealSeconds; // 1440 = total in-game minutes per day
        intensityChangeRate = 1f / (totalRealSeconds / 4f); // 1/4 of the day for rising and setting
        DelayTime = totalRealSeconds / 4f; // pause during full day or night
    }

    void Update()
    {
        UpdateGameTime();

        if (!isPaused)
        {
            if (increasing)
            {
                lightSource.intensity += intensityChangeRate * Time.deltaTime;
                NightSource.intensity -= intensityChangeRate * Time.deltaTime;
                LampLight.intensity -= intensityChangeRate * Time.deltaTime;

                if (lightSource.intensity >= targetIntensity && NightSource.intensity <= minIntensity)
                {
                    lightSource.intensity = targetIntensity;
                    NightSource.intensity = minIntensity;
                    LampLight.intensity = minIntensity;
                    increasing = false;
                    StartCoroutine(PauseCoroutine());
                }
            }
            else
            {
                lightSource.intensity -= intensityChangeRate * Time.deltaTime;
                NightSource.intensity += intensityChangeRate * Time.deltaTime;
                LampLight.intensity += intensityChangeRate * Time.deltaTime;

                if (lightSource.intensity <= minIntensity && NightSource.intensity >= targetIntensity)
                {
                    lightSource.intensity = minIntensity;
                    NightSource.intensity = targetIntensity;
                    LampLight.intensity = targetIntensity;
                    increasing = true;
                    StartCoroutine(PauseCoroutine());
                }
            }
        }
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

    private IEnumerator PauseCoroutine()
    {
        isPaused = true;
        yield return new WaitForSeconds(DelayTime);
        isPaused = false;
    }
}
