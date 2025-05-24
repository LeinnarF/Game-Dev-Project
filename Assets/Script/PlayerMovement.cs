using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

[System.Serializable]
public class FishData
{
    public string fishName;
    public Sprite fishSprite;
    public Rarity rarity;

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic
    }
}

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public float speed = 2f;
    public float sprintSpeed = 4f;

    private float x;
    private float y;
    private Vector2 input;
    private Vector2 lastMoveDirection = Vector2.down;

    private bool isMoving;

    public bool isFishing = false;
    public bool isInFishingSpot = false;
    private FishingSpot currentFishingSpot;
    private Coroutine fishingCoroutine;
    public bool waitingForPopup = false;
    public bool isInCameraMode = false;

    public float fishingRayDistance = 1.5f;

    [Header("Fish UI Pop-up")]
    public GameObject popUp;
    public GameObject fishPanel;
    public Text txtWindow;
    public Image imgWindow;
    public Animator popupAnimator;

    [Header("Fish List")]
    public List<FishData> fishList = new List<FishData>();

    private Color commonColor = Color.white;
    private Color uncommonColor = Color.green;
    private Color rareColor = new Color32(173, 216, 230, 255);
    private Color epicColor = new Color(221f / 255f, 160f / 255f, 221f / 255f);

    [Header("Logbook Settings")]
    public GameObject persistentObject2Prefab;
    private GameObject persistentObject2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        InitializePersistentObject2();
    }

    void InitializePersistentObject2()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "PersistentObject2" && obj.scene.IsValid())
            {
                persistentObject2 = obj;
                break;
            }
        }

        if (persistentObject2 == null)
        {
            foreach (GameObject obj in allObjects)
            {
                if (obj.CompareTag("Logbook") && obj.scene.IsValid())
                {
                    persistentObject2 = obj;
                    break;
                }
            }
        }

        if (persistentObject2 == null && persistentObject2Prefab != null)
        {
            persistentObject2 = Instantiate(persistentObject2Prefab);
            persistentObject2.name = "PersistentObject2";
            persistentObject2.tag = "Logbook";
            persistentObject2.SetActive(false);
        }
    }

    void Update()
    {
        HandleInput();
        Animate();
        CheckForFishingSpot();

        if (isFishing && input.magnitude > 0.1f)
        {
            StopFishing();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void HandleInput()
    {
        speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : 2f;

        if (!isFishing && !waitingForPopup && !isInCameraMode)
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
            if (x != 0) y = 0;
            input = new Vector2(x, y);

            if (input != Vector2.zero)
                lastMoveDirection = input;
        }
        else
        {
            x = 0;
            y = 0;
            input = Vector2.zero;
        }

        if (isInFishingSpot && Input.GetMouseButtonDown(0) && !waitingForPopup && !isInCameraMode)
        {
            ToggleFishing();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void Move()
    {
        rb.linearVelocity = input * speed;
    }

    void Animate()
    {
        isMoving = input.magnitude > 0.1f;

        if (isMoving)
        {
            anim.SetFloat("x", x);
            anim.SetFloat("y", y);
        }

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isFishing", isFishing);
    }

    void CheckForFishingSpot()
    {
        Vector2 direction = lastMoveDirection == Vector2.zero ? Vector2.down : lastMoveDirection;
        float offsetDistance = 1.5f;
        Vector2 origin = (Vector2)transform.position + direction * offsetDistance;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, fishingRayDistance, LayerMask.GetMask("Default"));

        if (hit.collider != null && hit.collider.CompareTag("FishingSpot"))
        {
            isInFishingSpot = true;
            currentFishingSpot = hit.collider.GetComponent<FishingSpot>();
        }
        else
        {
            isInFishingSpot = false;
            currentFishingSpot = null;
        }

        Debug.DrawRay(origin, direction * fishingRayDistance, Color.blue);
    }

    void ToggleFishing()
    {
        isFishing = !isFishing;
        if (isFishing && fishingCoroutine == null)
        {
            fishingCoroutine = StartCoroutine(FishingTimer());
        }
        else if (!isFishing)
        {
            StopFishing();
        }
    }

    void StopFishing()
    {
        isFishing = false;
        if (fishingCoroutine != null)
        {
            StopCoroutine(fishingCoroutine);
            fishingCoroutine = null;
        }
    }

    IEnumerator FishingTimer()
    {
        int waitTime = Random.Range(3, 21);
        yield return new WaitForSeconds(waitTime);

        if (currentFishingSpot != null && isFishing)
        {
            FishData caughtFish = GetRandomFish();

            if (caughtFish != null)
            {
                ShowFishPopup(caughtFish);
                AddFishToLogbook(caughtFish);

                isFishing = false;
                fishingCoroutine = null;
                waitingForPopup = true;

                if (popupAnimator != null)
                    popupAnimator.SetTrigger("PlayPopUp");

                yield return new WaitForSeconds(2f);
                fishPanel.SetActive(false);
                waitingForPopup = false;
            }
        }
    }

    FishData GetRandomFish()
    {
        float roll = Random.value;
        FishData.Rarity selectedRarity;

        if (roll < 0.5f)
            selectedRarity = FishData.Rarity.Common;
        else if (roll < 0.75f)
            selectedRarity = FishData.Rarity.Uncommon;
        else if (roll < 0.99f)
            selectedRarity = FishData.Rarity.Rare;
        else
            selectedRarity = FishData.Rarity.Epic;

        List<FishData> pool = fishList.FindAll(f => f.rarity == selectedRarity);
        return pool.Count == 0 ? null : pool[Random.Range(0, pool.Count)];
    }

    void ShowFishPopup(FishData fish)
    {
        if (txtWindow != null && imgWindow != null && fishPanel != null)
        {
            txtWindow.text = fish.fishName;
            imgWindow.sprite = fish.fishSprite;
            fishPanel.SetActive(true);

            switch (fish.rarity)
            {
                case FishData.Rarity.Common:
                    txtWindow.color = commonColor;
                    break;
                case FishData.Rarity.Uncommon:
                    txtWindow.color = uncommonColor;
                    break;
                case FishData.Rarity.Rare:
                    txtWindow.color = rareColor;
                    break;
                case FishData.Rarity.Epic:
                    txtWindow.color = epicColor;
                    break;
            }
        }
    }

    void AddFishToLogbook(FishData fish)
    {
        if (persistentObject2 == null)
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name == "PersistentObject2" && obj.scene.IsValid())
                {
                    persistentObject2 = obj;
                    break;
                }
            }

            if (persistentObject2 == null)
            {
                foreach (GameObject obj in allObjects)
                {
                    if (obj.CompareTag("Logbook") && obj.scene.IsValid())
                    {
                        persistentObject2 = obj;
                        break;
                    }
                }
            }

            if (persistentObject2 == null)
            {
                Debug.LogWarning("PersistentObject2 (Logbook) not found.");
                return;
            }
        }

        string fishSpriteName = fish.fishSprite.name;
        Match numberMatch = Regex.Match(fishSpriteName, @"\d+");

        if (numberMatch.Success && int.TryParse(numberMatch.Value, out int fishNumber))
        {
            string expectedSilhouteID = $"silhoute{fishNumber}";

            Image[] allImages = persistentObject2.GetComponentsInChildren<Image>(true);
            bool matchFound = false;

            foreach (Image img in allImages)
            {
                if (img.sprite != null)
                {
                    string spriteName = img.sprite.name.ToLower();
                    if (spriteName.Contains(expectedSilhouteID))
                    {
                        img.gameObject.SetActive(false);
                        Debug.Log($"Deactivated silhoute: {img.gameObject.name} ({spriteName})");
                        matchFound = true;
                        break;
                    }
                }

                if (!matchFound)
                {
                    string goName = img.gameObject.name.ToLower();
                    if (goName.Contains(expectedSilhouteID))
                    {
                        img.gameObject.SetActive(false);
                        Debug.Log($"Deactivated silhoute GameObject by name: {img.gameObject.name}");
                        matchFound = true;
                        break;
                    }
                }
            }

            if (!matchFound)
            {
                Debug.LogWarning($"No matching silhoute found for fish {fishNumber}");
            }
        }
        else
        {
            Debug.LogError($"Could not extract number from sprite: {fishSpriteName}");
        }
    }
}
