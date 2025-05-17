using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int SceneToLoad;
    public GameObject FaidOut;

    public void OnContinue()
    {
        // Load the last saved game or the game scene
        // Assuming the game scene is named "GameScene"
        SceneManager.LoadScene("Main");
    }

    public void OnNewGame()
    {
       StartCoroutine(loadMain());
    }
    IEnumerator loadMain()
    {
        Instantiate(FaidOut);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Single);
    }
    public void OnSettings()
    {
        // Load the settings scene
        // Assuming you have a settings scene named "SettingsScene"
        //SceneManager.LoadScene("SettingsScene");
    }

    public void OnExit()
    {
       Application.Quit();
        // If you are running in the editor, use this line to stop playing
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif 
    }
}