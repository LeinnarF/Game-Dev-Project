using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button continueButton;
    public Button newGameButton;
    public Button settingsButton;
    public Button exitButton;

    private void Start()
    {
        // Add listeners to the buttons
        continueButton.onClick.AddListener(OnContinue);
        newGameButton.onClick.AddListener(OnNewGame);
        settingsButton.onClick.AddListener(OnSettings);
        exitButton.onClick.AddListener(OnExit);
    }

    private void OnContinue()
    {
        // Load the last saved game or the game scene
        // Assuming the game scene is named "GameScene"
        SceneManager.LoadScene("Main");
    }

    private void OnNewGame()
    {
        // Load the game scene for a new game
        SceneManager.LoadScene("Main");
    }

    private void OnSettings()
    {
        // Load the settings scene
        // Assuming you have a settings scene named "SettingsScene"
        //SceneManager.LoadScene("SettingsScene");
    }

    private void OnExit()
    {
        // Exit the application
        // Application.Quit();
        // #if UNITY_EDITOR
        // UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
        // #endif
    }
}