using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int SceneToLoadNewgame;
    public int SceneToLoadContinue;
    public GameObject FaidOut;

    public void OnContinue()
    {
       StartCoroutine(loadCon());
    }
    IEnumerator loadCon()
    {
        Instantiate(FaidOut);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneToLoadContinue, LoadSceneMode.Single);
    }

    public void OnNewGame()
    {
       StartCoroutine(loadMain());
    }
    IEnumerator loadMain()
    {
        Instantiate(FaidOut);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneToLoadNewgame, LoadSceneMode.Single);
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