using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;     // For PersistentObject
    public GameObject prefabToSpawn2;    // For PersistentObject2 (Canvas)

    public string uniqueObjectName = "PersistentObject";
    public string uniqueObjectName2 = "PersistentObject2";
    public bool isActive = false; // To control the activation of the second object
    private GameObject persistentObject;
    private GameObject persistentObject2;

    void Update()
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
        if (persistentObject2 == null && prefabToSpawn2 != null &&GameObject.Find("PersistentObject2") == null)
        {
            persistentObject2 = Instantiate(prefabToSpawn2);
            persistentObject2.name = uniqueObjectName2;
            DontDestroyOnLoad(persistentObject2);
            persistentObject2.SetActive(false); // Start disabled
        }

        // Check if user presses the "B" key
        if (Input.GetKeyDown(KeyCode.B) && GameObject.Find("InventoryMenu") == null && GameObject.Find("CameraOverlay") == null && !isActive)
        {
            isActive = true; // Toggle the flag
            ActivateCanvas();
        }
        else if (Input.GetKeyDown(KeyCode.B) && GameObject.Find("PersistentObject2") != null)
        {
            DeactivateCanvas();
            isActive = false;
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
