using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject prefabToSpawn;     // For PersistentObject
    public GameObject prefabToSpawn2;
    public GameObject prefabToSpawn3;    // For PersistentObject2 (Canvas) and PersistentObject3 (for future use)

    [Header("Unique Object Names")]
    public string uniqueObjectName = "PersistentObject";
    public string uniqueObjectName2 = "PersistentObject2";
    public string uniqueObjectName3 = "PersistentObject3"; // For future use

    private static GameObject persistentObject;
    private static GameObject persistentObject2;
    private static GameObject persistentObject3; // For future use

    private static ObjectSpawner instance;

    private bool isActive = false;

    void Awake()
    {
        // Singleton pattern to prevent multiple ObjectSpawner instances
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Handle PersistentObject
        if (persistentObject == null)
        {
            persistentObject = GameObject.Find(uniqueObjectName);
            if (persistentObject == null && prefabToSpawn != null)
            {
                persistentObject = Instantiate(prefabToSpawn);
                persistentObject.name = uniqueObjectName;
            }
            DontDestroyOnLoad(persistentObject);
        }

        // Handle PersistentObject2 (Canvas)
        if (persistentObject2 == null)
        {
            persistentObject2 = GameObject.Find(uniqueObjectName2);
            if (persistentObject2 == null && prefabToSpawn2 != null)
            {
                persistentObject2 = Instantiate(prefabToSpawn2);
                persistentObject2.name = uniqueObjectName2;
                persistentObject2.SetActive(false); // Start disabled
            }
            DontDestroyOnLoad(persistentObject2);
        }
        // Handle PersistentObject2 (Canvas)
        if (persistentObject3 == null)
        {
            persistentObject3 = GameObject.Find(uniqueObjectName3);
            if (persistentObject3 == null && prefabToSpawn3 != null)
            {
                persistentObject3 = Instantiate(prefabToSpawn3);
                persistentObject3.name = uniqueObjectName2;
                persistentObject3.SetActive(false); // Start disabled
            }
            DontDestroyOnLoad(persistentObject3);
        }
    }

    void Update()
    {
        // Re-find in case references are lost (should rarely happen)
        if (persistentObject2 == null)
        {
            persistentObject2 = GameObject.Find(uniqueObjectName2);
        }

        // Block toggle if UI overlays are open
        GameObject cameraUI = GameObject.Find("CameraOverlay");
        GameObject inventoryUI = GameObject.Find("InventoryMenu");
        GameObject logbookUI = GameObject.Find("PersistentObject2");

        bool isBlocked = (cameraUI != null && cameraUI.activeInHierarchy) || (inventoryUI != null && inventoryUI.activeInHierarchy);

        // Debug log to check if the input is being registered
        if (Input.GetKeyDown(KeyCode.B) && !isBlocked)
        {
            Debug.Log("B key pressed. Toggling PersistentObject2.");
            isActive = !isActive;
            persistentObject2.SetActive(isActive);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("B key pressed but toggling is blocked by UI.");
        }



        // Re-find in case references are lost (should rarely happen)
        if (persistentObject3 == null)
        {
            persistentObject3 = GameObject.Find(uniqueObjectName3);
        }

        bool isBlocked1 = (cameraUI != null && cameraUI.activeInHierarchy) || (logbookUI != null && logbookUI.activeInHierarchy);

        // Debug log to check if the input is being registered
        if (Input.GetKeyDown(KeyCode.E) && !isBlocked)
        {
            Debug.Log("E key pressed. Toggling PersistentObject2.");
            isActive = !isActive;
            persistentObject3.SetActive(isActive);
        }else if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("B key pressed but toggling is blocked by UI.");
        }
    }
    }
