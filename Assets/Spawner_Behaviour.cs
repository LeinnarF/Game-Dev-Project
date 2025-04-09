using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject AnimalPrefab;
    public float SpawnTime;
    private Camera mainCamera;
    private bool isSpawning = false;
    void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;
        
    }

        void Update()
        {
            if (IsInsideCamera() && AnimalPrefab != null && !isSpawning && !GameObject.FindGameObjectWithTag("Animal"))
            {          
                    Invoke(nameof(SpawnAnimal), SpawnTime);
                    isSpawning = true;
            }
        
        }
   
    void SpawnAnimal()
    {
        Instantiate(AnimalPrefab, transform.position,Quaternion.identity);
        isSpawning = false;
    }
    private bool IsInsideCamera()
    {
        // Get the object's position in viewport coordinates
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // Check if the object is inside the viewport
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1;
    }

}

