using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject AnimalPrefab;     // Drag your Buuni prefab here
    public float SpawnTime = 2f;        // Delay before spawning
    public float DespawnDelay = 120f;   // 2 minutes in seconds

    private GameObject animalInstance;
    private bool hasSpawned = false;
    private bool playerInside = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called with: " + other.name + ", Tag: " + other.tag);

        if (other.CompareTag("Player"))
        {
            playerInside = true;

            if (!hasSpawned)
            {
                hasSpawned = true;
                // For testing, spawn immediately:
                SpawnAnimal();
                // Or use Invoke if you want a delay:
                // Invoke(nameof(SpawnAnimal), SpawnTime);
                Debug.Log("Player entered aura. Animal will spawn.");
            }
            else
            {
                CancelInvoke(nameof(DespawnAnimal));
                Debug.Log("Player re-entered aura. Despawn canceled.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("OnTriggerExit2D called with: " + other.name + ", Tag: " + other.tag);

        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (hasSpawned && animalInstance != null)
            {
                Debug.Log("Player exited aura. Despawning in " + DespawnDelay + "s.");
                Invoke(nameof(DespawnAnimal), DespawnDelay);
            }
        }
    }

    private void SpawnAnimal()
    {
        if (AnimalPrefab == null)
        {
            Debug.LogError("AnimalPrefab not assigned!");
            return;
        }

        if (animalInstance == null)
        {
            animalInstance = Instantiate(AnimalPrefab, transform.position, Quaternion.identity);
            Debug.Log("AnimalPrefab spawned!");
        }
    }

    private void DespawnAnimal()
    {
        if (!playerInside && animalInstance != null)
        {
            Destroy(animalInstance);
            animalInstance = null;
            hasSpawned = false;
            Debug.Log("AnimalPrefab despawned after player left aura.");
        }
    }
}
