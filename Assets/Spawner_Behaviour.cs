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
              
            }
            else
            {
                CancelInvoke(nameof(DespawnAnimal));
                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
       

        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (hasSpawned && animalInstance != null)
            {
                Invoke(nameof(DespawnAnimal), DespawnDelay);
            }
        }
    }

    private void SpawnAnimal()
    {
        if (AnimalPrefab == null)
        {
        
            return;
        }

        if (animalInstance == null)
        {
            animalInstance = Instantiate(AnimalPrefab, transform.position, Quaternion.identity);
       
        }
    }

    private void DespawnAnimal()
    {
        if (!playerInside && animalInstance != null)
        {
            Destroy(animalInstance);
            animalInstance = null;
            hasSpawned = false;
          
        }
    }
}
