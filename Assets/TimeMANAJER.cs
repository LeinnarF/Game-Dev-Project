using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner Instance { get; private set; }

    public GameObject prefabToSpawn;     // For PersistentObject
    public GameObject prefabToSpawn2;    // For PersistentObject2 (Canvas)

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
        DontDestroyOnLoad(gameObject); // Make this ObjectSpawner persistent
    }

    void Start()
    {
        // Prevent duplicate persistentObject
        persistentObject = GameObject.Find(uniqueObjectName);
        if (persistentObject == null && prefabToSpawn != null)
        {
            persistentObject = Instantiate(prefabToSpawn);
            persistentObject.name = uniqueObjectName;
            DontDestroyOnLoad(persistentObject);
        }

        // Prevent duplicate persistentObject2
        persistentObject2 = GameObject.Find(uniqueObjectName2);
        if (persistentObject2 == null && prefabToSpawn2 != null)
        {
            persistentObject2 = Instantiate(prefabToSpawn2);
            persistentObject2.name = uniqueObjectName2;
            DontDestroyOnLoad(persistentObject2);
            persistentObject2.SetActive(false); // Start hidden
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) &&
            GameObject.Find("InventoryMenu") == null &&
            GameObject.Find("CameraOverlay") == null)
        {
            if (persistentObject2 != null)
            {
                isActive = !isActive;
                persistentObject2.SetActive(isActive);
            }
        }
    }
}
