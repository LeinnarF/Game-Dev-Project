using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;     // For PersistentObject
    public GameObject prefabToSpawn2;    // For PersistentObject2 (Canvas)

    public string uniqueObjectName = "PersistentObject";
    public string uniqueObjectName2 = "PersistentObject2";
    public bool isActive; // To control the activation of the second object
    private GameObject persistentObject;
    private GameObject persistentObject2;

    void Start()
    {
        // Look for existing objects by name
        persistentObject = GameObject.Find(uniqueObjectName);
        persistentObject2 = GameObject.Find(uniqueObjectName2);

        // Spawn PersistentObject if not found
        if (persistentObject == null && prefabToSpawn != null)
        {
            persistentObject = Instantiate(prefabToSpawn);
            persistentObject.name = uniqueObjectName;
            DontDestroyOnLoad(persistentObject);
        }

        // Spawn PersistentObject2 if not found
        if (persistentObject2 == null && prefabToSpawn2 != null)
        {
            persistentObject2 = Instantiate(prefabToSpawn2);
            persistentObject2.name = uniqueObjectName2;
            DontDestroyOnLoad(persistentObject2);
            persistentObject2.SetActive(false); // Start disabled
        }
    }

    void Update()
    {
        // Check if user presses the "B" key
        if (Input.GetKeyDown(KeyCode.B) && GameObject.Find("InventoryMenu") == null && GameObject.Find("CameraOverlay") == null)
        {
            Time.timeScale = 1;
            isActive = false; // Reset the flag when the key is pressed
            ActivateCanvas();
        }
        else if (Input.GetKeyDown(KeyCode.B) && !isActive)
        {
            Time.timeScale = 0;
            DeactivateCanvas();
        }

    }

    public void ActivateCanvas()
    {
        if (persistentObject2 != null)
        {
            persistentObject2.SetActive(true);
            isActive = true; // Set the flag to true when activated
            Debug.Log("PersistentObject2 (Canvas) activated by pressing B.");
        }
    }
    
    public void DeactivateCanvas()
    {
        if (persistentObject2 != null)
        {
            persistentObject2.SetActive(false);
            isActive = false; // Set the flag to false when deactivated
            Debug.Log("PersistentObject2 (Canvas) deactivated.");
        }
    }
        
}
