using UnityEngine;
using UnityEngine.SceneManagement;
public class EnterCabin : MonoBehaviour
{
    public bool IsInDoor = false;
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

    void Entry()
    {
        if (IsInDoor == true)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

}
