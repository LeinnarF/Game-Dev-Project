using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject AnimalPrefab;
    public float SpawnTime;
    public float SpawnDelayTime;
    private Camera mainCamera;
    private bool isSpawning = false;
    void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;
        
    }

        void Update()
        {
            if (IsInsideCamera() && !isSpawning )
            {
                // Check if the object is inside the camera's view
                // If it is, start spawning animals
                isSpawning = true;
                Invoke(nameof(SpawnAnimal), SpawnTime);
            }
            else if (IsInsideCamera() && isSpawning)
            {
                // If the object is outside the camera's view, stop spawning animals
                CancelInvoke(nameof(SpawnAnimal));
            }
            if(IsInsideCamera() && GameObject.Find(AnimalPrefab.name + "(Clone)")==null && isSpawning)
            {
                isSpawning = false;
            }
        
        }
   
    void SpawnAnimal()
    {
        SpawnDelayTime -= Time.deltaTime;
        if (SpawnDelayTime <= 0)
        {
        Instantiate(AnimalPrefab, transform.position,Quaternion.identity);
        SpawnDelayTime = SpawnTime;
        }
    }
    private bool IsInsideCamera()
    {
        // Get the object's position in viewport coordinates
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // Check if the object is inside the viewport
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1;
    }

}

