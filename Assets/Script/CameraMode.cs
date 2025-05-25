using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.Serialization;

 [System.Serializable]
    public class AnimalData
    {
        public string imageSuffix;
        public AnimalRarity rarity;
        public Sprite animalSprite; // You can assign this in the inspector or load it dynamically

        public AnimalData(string suffix, AnimalRarity animalRarity)
        {
            imageSuffix = suffix;
            rarity = animalRarity;
        }
    }
    

    public enum AnimalRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic
    }
public class CameraMode : MonoBehaviour
{
    private readonly Dictionary<string, AnimalData> animalDatabase = new()
    {
        { "Buuni", new AnimalData("1", AnimalRarity.Common) },
        { "Daddy_Bear", new AnimalData("22", AnimalRarity.Epic) },
        { "Moose", new AnimalData("4", AnimalRarity.Rare) },
        { "Owl", new AnimalData("6", AnimalRarity.Uncommon) },
        { "Frog", new AnimalData("3", AnimalRarity.Common) },
        { "Fox", new AnimalData("2", AnimalRarity.Uncommon) },
        { "Wolf", new AnimalData("25", AnimalRarity.Uncommon) },
        { "Bisen", new AnimalData("30", AnimalRarity.Rare) },
        { "blue", new AnimalData("15", AnimalRarity.Epic) },
        { "Caribou", new AnimalData("32", AnimalRarity.Rare) },
        { "duck", new AnimalData("23", AnimalRarity.Epic) },
        { "Justin", new AnimalData("31", AnimalRarity.Rare) },
        { "Martin", new AnimalData("14", AnimalRarity.Epic) },
        { "Reindeer", new AnimalData("5", AnimalRarity.Rare) },
        { "Xbill", new AnimalData("33", AnimalRarity.Epic) }
       
    };
    private readonly Dictionary<string, string> animalNameToImageSuffix = new()
    {
        { "Buuni", "1" },
        { "Daddy_Bear", "22" },
        { "Moose", "4" },
        { "Owl", "6" },
        { "Frog", "3" },
        { "Fox", "2" },
        { "Wolf", "25" },
        { "Bisen","30" },
        { "blue", "15"},
        { "Caribou", "32"},
        { "duck", "23"},
        { "Justin", "31" },
        { "Martin", "14" },
        { "Reindeer", "5" },
        { "Xbill","33" }
    };
    

    public float moveSpeed = 5f;
    private PlayerMovement player;

    public GameObject Logook;
    private Camera mainCam;

    private Dictionary<string, (Image question, Image unknown)> animalImages = new();
    private HashSet<string> seenAnimals = new();
    private HashSet<string> capturedAnimals = new();

    private GameObject CameraOverlay; // Camera overlay GameObject
    public GameObject CameraWindowPanel; // Image inside CameraOverlay
    public Animator CameraWindowAnimator;
    public Text TxtWindow;
    public Image ImageWindow;
    private Animator cameraOverlayAnimator;

    // Color settings for different animal rarities/types
    public Color commonColor = Color.white;
    public Color uncommonColor = Color.green;
    public Color rareColor = Color.blue;
    public Color epicColor = Color.magenta;

    // Map animal names to image number suffix and rarity


    // Legacy dictionary for backward compatibility




    void Start()
    {
        player = FindFirstObjectByType<PlayerMovement>();
        mainCam = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>();

        StartCoroutine(FindLogbookAndImages());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (CameraOverlay != null)
            {
                bool isActive = !CameraOverlay.activeSelf;
                CameraOverlay.SetActive(isActive);

                if (player != null)
                    player.isInCameraMode = isActive;

                if (isActive)
                    SnapToPlayer();
            }
        }

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
                if (cameraOverlayAnimator != null)
                {
                    cameraOverlayAnimator.SetTrigger("Camera");
                }

                string capturedAnimal = TryCaptureAnimal();
                if (!string.IsNullOrEmpty(capturedAnimal))
                {
                    Debug.Log($"Captured new animal: {capturedAnimal}");
                    ShowAnimalPopup(capturedAnimal);
                }
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

    string TryCaptureAnimal()
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

                    return animalName; // Return the captured animal name
                }
            }
        }

        return string.Empty; // No animal captured
    }

    void ShowAnimalPopup(string animalName)
    {
        if (TxtWindow != null && ImageWindow != null && CameraWindowPanel != null && animalDatabase.ContainsKey(animalName))
        {
            AnimalData animalData = animalDatabase[animalName];

            // Set animal name
            TxtWindow.text = animalName.Replace("_", " "); // Replace underscores with spaces for display

            // Set animal image (you'll need to assign sprites to the AnimalData or load them dynamically)
            if (animalData.animalSprite != null)
            {
                ImageWindow.sprite = animalData.animalSprite;
            }
            else
            {
                // Try to load sprite from Resources folder based on animal name
                Sprite loadedSprite = Resources.Load<Sprite>($"Animals/{animalName}");
                if (loadedSprite != null)
                {
                    ImageWindow.sprite = loadedSprite;
                    animalData.animalSprite = loadedSprite; // Cache it for next time
                }
            }

            Debug.Log($"Showing popup for animal: {animalName}");
            CameraWindowPanel.SetActive(true);

            // Set text color based on rarity
            switch (animalData.rarity)
            {
                case AnimalRarity.Common:
                    TxtWindow.color = commonColor;
                    break;
                case AnimalRarity.Uncommon:
                    TxtWindow.color = uncommonColor;
                    break;
                case AnimalRarity.Rare:
                    TxtWindow.color = rareColor;
                    break;
                case AnimalRarity.Epic:
                    TxtWindow.color = epicColor;
                    break;
            }

            // Trigger animation if available
            if (CameraWindowAnimator != null)
            {
                CameraWindowAnimator.SetTrigger("PlayPopUp"); // You'll need to create this trigger in your animator
            }

            // Auto-hide popup after a few seconds (optional)
            StartCoroutine(HidePopupAfterDelay(3f));
        }
    }

    IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (CameraWindowPanel != null)
        {
            CameraWindowPanel.SetActive(false);
        }
    }

    // Public method to manually hide the popup (can be called by UI button)
    public void HideAnimalPopup()
    {
        if (CameraWindowPanel != null)
        {
            CameraWindowPanel.SetActive(false);
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