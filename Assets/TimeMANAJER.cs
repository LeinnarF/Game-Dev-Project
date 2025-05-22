using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
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
        // Handle PersistentObject
        persistentObject = GameObject.Find(uniqueObjectName);
        if (persistentObject == null && prefabToSpawn != null)
        {
            persistentObject = Instantiate(prefabToSpawn);
            persistentObject.name = uniqueObjectName;
            DontDestroyOnLoad(persistentObject);
        }

        // Handle PersistentObject2 (Canvas)
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
        // Re-find in case references are lost
        if (persistentObject2 == null)
        {
            persistentObject2 = GameObject.Find(uniqueObjectName2);
        }

        // Block toggle if UI overlays are open
        GameObject cameraUI = GameObject.Find("CameraOverlay");
        GameObject inventoryUI = GameObject.Find("Inventory");

        bool isBlocked = (cameraUI != null && cameraUI.activeInHierarchy) ||
                         (inventoryUI != null && inventoryUI.activeInHierarchy);

        if (Input.GetKeyDown(KeyCode.B) && !isBlocked && persistentObject2 != null)
        {
            isActive = !isActive;
            persistentObject2.SetActive(isActive);
        }
    }
}
