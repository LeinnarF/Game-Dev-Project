using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStamina : MonoBehaviour
{
    public int stamina;
    public int maxStamina = 100;
    public TMP_Text staminaText;

    public string bedSceneName = "Inside Cabin"; // Set this to your actual bed scene name
    private bool isRespawning = false;

    void Start()
    {
        stamina = maxStamina;
        UpdateStaminaUI();

        // When reloading into bed scene, go to spawn
        if (SpawnManager.spawnPosition != Vector3.zero)
        {
            transform.position = SpawnManager.spawnPosition;
        }
    }

    public void TakeDamage(int amount)
    {
        stamina -= amount;
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        UpdateStaminaUI();

        if (stamina <= 0 && !isRespawning)
        {
            isRespawning = true;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(bedSceneName);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Respawn the player next to bed
        transform.position = SpawnManager.spawnPosition;
        stamina = maxStamina;
        UpdateStaminaUI();

        SceneManager.sceneLoaded -= OnSceneLoaded;
        isRespawning = false;
    }

    void UpdateStaminaUI()
    {
        if (staminaText != null)
        {
            staminaText.text = "Stamina: " + stamina.ToString();
        }
    }

    public void ChangeStamina(int amount)
    {
        stamina += amount;
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        UpdateStaminaUI();
    }

}
