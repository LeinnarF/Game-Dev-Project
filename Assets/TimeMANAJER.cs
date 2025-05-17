using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public string uniqueObjectName = "PersistentObject"; // optional: assign a name for identification

    void Start()
    {
        // Look for existing object by name
        GameObject existing = GameObject.Find(uniqueObjectName);

        if (existing == null)
        {
            // Spawn the prefab
            GameObject newObject = Instantiate(prefabToSpawn);
            newObject.name = uniqueObjectName; // give it a unique name

            DontDestroyOnLoad(newObject); // make it persist
        }
        else
        {
            Debug.Log("Object already exists. Skipping spawn.");
        }
    }
}