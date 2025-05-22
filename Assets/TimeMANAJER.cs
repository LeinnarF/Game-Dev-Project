using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner Instance { get; private set; }

    [Header("Prefabs")]
    public GameObject prefabToSpawn;     // For PersistentObject
    public GameObject prefabToSpawn2;    // For PersistentObject2 (Canvas)

    [Header("Unique Object Names")]
    public string uniqueObjectName = "PersistentObject";
    public string uniqueObjectName2 = "PersistentObject2";

    private GameObject persistentObject;
    private GameObject persistentObject2;

    private bool isActive = false;

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Make ObjectSpawner persistent
    }

    void Start()
    {
        // Spawn PersistentObject if not already present
        persistentObject = GameObject.Find(uniqueObjectName);
        if (persistentObject == null && prefabToSpawn != null)
        {
            persistentObject = Instantiate(prefabToSpawn);
            persistentObject.name = uniqueObjectName;
            DontDestroyOnLoad(persistentObject);
        }

        // Spawn PersistentObject2 if not already present
        persistentObject2 = GameObject.Find(uniqueObjectName2);
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
        // Only toggle if neither Camera nor Inventory is active
        GameObject cameraUI = GameObject.Find("CameraOverlay");
        GameObject inventoryUI = GameObject.Find("Inventory");
        bool isBlocked = (cameraUI != null && cameraUI.activeInHierarchy) ||
                         (inventoryUI != null && inventoryUI.activeInHierarchy);

        if (Input.GetKeyDown(KeyCode.B) && !isBlocked)
        {
            if (persistentObject2 != null)
            {
                isActive = !isActive;
                persistentObject2.SetActive(isActive);
            }
        }
    }
}
