using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Use this for TextMeshPro
using UnityEngine.SceneManagement; // Add this for scene loading

public class PlayerStamina : MonoBehaviour
{
    public int stamina;
    public int maxStamina = 100;
    public TMP_Text staminaText; // Drag your UI text object here in Inspector
    public string bedSceneName = "Inside Cabin"; // Set this to your actual bed scene name
    private bool isRespawning = false;
    void Start()
    {
        stamina = maxStamina;
        UpdateStaminaUI();
    }

    public void TakeDamage(int amount)
    {
        stamina -= amount;
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        UpdateStaminaUI();

         if (stamina <= 0 && !isRespawning)
        {
        GameFlags.isRespawning = true; // 💡 Remember we fainted
        isRespawning = true;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(bedSceneName);
        }
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameFlags.isRespawning)
        {
            transform.position = SpawnManager.spawnPosition;
            GameFlags.isRespawning = false;
            stamina = maxStamina;
            UpdateStaminaUI();
        }

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
}
