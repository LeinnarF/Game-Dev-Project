using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EnterCabin : MonoBehaviour
{
    public bool IsInDoor = false;
    public GameObject FadeOut;
    public string sceneToLoad; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Entry();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the object entering the trigger has the "Player" tag
        {
            IsInDoor = true; 
        }
    }

    public void Entry()
    {
        if (IsInDoor == true)
        {
            StartCoroutine(load());
        }
    }

    IEnumerator load()
    {
        Instantiate(FadeOut);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
