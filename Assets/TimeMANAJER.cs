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

    void Start()
    {
        persistentObject = GameObject.Find(uniqueObjectName);
        persistentObject2 = GameObject.Find(uniqueObjectName2);
        isActive = false; // Initialize the flag
        // Spawn PersistentObject2 if not found
        if (persistentObject2 == null && prefabToSpawn2 != null)
        {
            persistentObject2 = Instantiate(prefabToSpawn2);
            persistentObject2.name = uniqueObjectName2;
            DontDestroyOnLoad(persistentObject2);
            persistentObject2.SetActive(false); // Start disabled
        }
        
        // Spawn PersistentObject if not found
        if (persistentObject == null && prefabToSpawn != null)
        {
            persistentObject = Instantiate(prefabToSpawn);
            persistentObject.name = uniqueObjectName;
            DontDestroyOnLoad(persistentObject);
        }
    }
    void Update()
    {
        // Look for existing objects by name
        persistentObject = GameObject.Find(uniqueObjectName);
        persistentObject2 = GameObject.Find(uniqueObjectName2);

        // Check if user presses the "B" key
        if (Input.GetKeyDown(KeyCode.B) && GameObject.Find("InventoryMenu") == null && GameObject.Find("CameraOverlay") == null )
        {
            isActive = true; // Toggle the flag
            persistentObject2.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.B) && isActive == true)
        {
            persistentObject2.SetActive(false);
            isActive = false;
        }
        

    }
        
}
