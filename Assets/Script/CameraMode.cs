using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CameraMode : MonoBehaviour
{
    public float moveSpeed = 5f;
    private PlayerMovement player;

    public GameObject Logook;
    private Camera mainCam;

    private Dictionary<string, (Image question, Image unknown)> animalImages = new();
    private HashSet<string> seenAnimals = new();
    private HashSet<string> capturedAnimals = new();

    // Map animal names to image number suffix
    private readonly Dictionary<string, string> animalNameToImageSuffix = new()
    {
        { "Buuni", "1" },
        { "Daddy_Bear", "22" },
        { "Moose", "4" },
        { "Owl", "6" },
        { "Frog", "3" },
        { "Fox", "2" },
        { "Wolf", "25" }
    };

    void Start()
    {
        player = FindFirstObjectByType<PlayerMovement>();
        mainCam = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>();
        if (mainCam == null)
            Debug.LogError("Main Camera not found. Make sure it's tagged 'MainCamera'.");

        StartCoroutine(FindLogbookAndImages());
    }

    void Update()
    {
        if (player != null && player.isInCameraMode)
        {
            Vector3 move = new Vector3(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical"),
                0f
            );
            transform.position += move.normalized * moveSpeed * Time.deltaTime;

            CheckForAnimalInView();

            if (Input.GetMouseButtonDown(0))
            {
                TryCaptureAnimal();
            }
        }
    }

    public void SnapToPlayer()
    {
        if (player != null)
        {
            Vector3 newPos = player.transform.position;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
    }

    void CheckForAnimalInView()
    {
        GameObject[] allAnimals = GameObject.FindGameObjectsWithTag("Animal");
        GameObject[] hostileAnimals = GameObject.FindGameObjectsWithTag("Hostile");
        GameObject[] flyingAnimals = GameObject.FindGameObjectsWithTag("Flying Animal");

        List<GameObject> all = new();
        all.AddRange(allAnimals);
        all.AddRange(hostileAnimals);
        all.AddRange(flyingAnimals);

        foreach (GameObject animal in all)
        {
            foreach (var kvp in animalNameToImageSuffix)
            {
                string animalName = kvp.Key;
                string suffix = kvp.Value;

                if (animal.name.Contains(animalName) && IsInView(animal) && !seenAnimals.Contains(animalName))
                {
                    seenAnimals.Add(animalName);
                    if (animalImages.ContainsKey(animalName))
                        MakeImageTransparent(animalImages[animalName].question, $"question ({suffix})");
                }
            }
        }
    }

    void TryCaptureAnimal()
    {
        GameObject[] allAnimals = GameObject.FindGameObjectsWithTag("Animal");
        GameObject[] hostileAnimals = GameObject.FindGameObjectsWithTag("Hostile");
        GameObject[] flyingAnimals = GameObject.FindGameObjectsWithTag("Flying Animal");

        List<GameObject> all = new();
        all.AddRange(allAnimals);
        all.AddRange(hostileAnimals);
        all.AddRange(flyingAnimals);

        foreach (GameObject animal in all)
        {
            foreach (var kvp in animalNameToImageSuffix)
            {
                string animalName = kvp.Key;
                string suffix = kvp.Value;

                if (animal.name.Contains(animalName) && IsInView(animal) && !capturedAnimals.Contains(animalName))
                {
                    capturedAnimals.Add(animalName);
                    if (animalImages.ContainsKey(animalName))
                        MakeImageTransparent(animalImages[animalName].unknown, $"unknown ({suffix})");
                }
            }
        }
    }

    bool IsInView(GameObject obj)
    {
        if (mainCam == null) return false;

        Vector3 viewportPos = mainCam.WorldToViewportPoint(obj.transform.position);
        return viewportPos.z > 0 &&
               viewportPos.x >= 0 && viewportPos.x <= 1 &&
               viewportPos.y >= 0 && viewportPos.y <= 1;
    }

    void MakeImageTransparent(Image img, string label)
    {
        if (img != null)
        {
            Color color = img.color;
            color.a = 0f;
            img.color = color;
            Debug.Log($"{label} image made transparent.");
        }
        else
        {
            Debug.LogWarning($"Image for '{label}' not assigned.");
        }
    }

    IEnumerator FindLogbookAndImages()
    {
        yield return new WaitForSeconds(0.1f);

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "PersistentObject2")
            {
                Logook = obj;
                break;
            }
        }

        if (Logook == null)
        {
            Debug.LogWarning("Logbook 'PersistentObject2' not found.");
            yield break;
        }

        Image[] allImages = Logook.GetComponentsInChildren<Image>(true);
        foreach (var kvp in animalNameToImageSuffix)
        {
            string name = kvp.Key;
            string suffix = kvp.Value;

            Image questionImg = null;
            Image unknownImg = null;

            foreach (Image img in allImages)
            {
                if (img.gameObject.name == $"question ({suffix})")
                    questionImg = img;
                else if (img.gameObject.name == $"unknown ({suffix})")
                    unknownImg = img;
            }

            if (questionImg == null)
                Debug.LogWarning($"Image 'question ({suffix})' not found.");
            if (unknownImg == null)
                Debug.LogWarning($"Image 'unknown ({suffix})' not found.");

            animalImages[name] = (questionImg, unknownImg);
        }
    }
}
