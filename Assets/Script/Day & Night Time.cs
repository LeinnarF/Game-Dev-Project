using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class LightIntensityController : MonoBehaviour
{
    public Light2D lightSource; // Reference to the Light component
    public Light2D NightSource; // Reference to the second Light component (if needed)
    public float intensityChangeRate = 0.01f; // Amount to change intensity each second
    private float targetIntensity = 1f; // Maximum intensity
    private float minIntensity = 0f; // Minimum intensity
    public float DelayTime;
    private bool increasing = true; // Flag to check if we are increasing or decreasing intensity
    private bool isPaused = false; // Flag to check if the script is paused

    void Start()
    {
        if (lightSource == null)
        {
            lightSource = GetComponent<Light2D>(); // Get the Light component if not assigned
        }
    }

    void Update()
    {
        if (!isPaused)
        {
            if (increasing)
            {
                // Increase intensity
                lightSource.intensity += intensityChangeRate * Time.deltaTime;
                NightSource.intensity -= intensityChangeRate * Time.deltaTime; // Increase the second light's intensity as well
                if (lightSource.intensity >= targetIntensity && NightSource.intensity <= minIntensity)
                {
                    NightSource.intensity = minIntensity; // Clamp to min intensity// Clamp to max intensity
                    lightSource.intensity = targetIntensity; // Clamp to max intensity
                    increasing = false; // Switch to decreasing
                    StartCoroutine(PauseCoroutine()); // Start pause when reaching max intensity
                }
            }
            else
            {
                // Decrease intensity
                lightSource.intensity -= intensityChangeRate * Time.deltaTime;
                NightSource.intensity += intensityChangeRate * Time.deltaTime; // Decrease the second light's intensity as well
                if (lightSource.intensity <= minIntensity && NightSource.intensity >= targetIntensity)
                {
                    lightSource.intensity = minIntensity;
                    NightSource.intensity = targetIntensity; // Clamp to min intensity
                    increasing = true; // Switch to increasing
                    StartCoroutine(PauseCoroutine()); // Start pause when reaching min intensity
                }
            }
        }
    }

    private IEnumerator PauseCoroutine()
    {
        isPaused = true; // Set the paused flag
        yield return new WaitForSeconds(DelayTime); // Wait for 1 minute (60 seconds)
        isPaused = false; // Reset the paused flag
    }
}