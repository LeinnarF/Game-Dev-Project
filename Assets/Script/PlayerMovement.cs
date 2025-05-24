using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private SpriteRenderer sr;

    public SpriteRenderer cmeraSprite;
    public GameObject cameraOverlay;

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
    private bool fishCaught = false;
    public bool isInCameraMode = false;
    public bool waitingForPopup = false;

    public float fishingRayDistance = 1.5f;

    [Header("Fish UI Pop-up")]
    public GameObject popUp;         // Root GameObject (Pop-up lowerleft)
    public GameObject fishPanel;     // Inner Panel (lowerleft)
    public Text txtWindow;           // Legacy Text (Txt_window)
    public Image imgWindow;          // Image (Img_window)
    public Animator popupAnimator;   // Animator on Pop-up lowerleft

    [Header("Fish List")]
    public List<FishData> fishList = new List<FishData>();

    private Color commonColor = Color.white;
    private Color uncommonColor = Color.green;
    private Color rareColor = new Color32(173, 216, 230, 255); // Light blue using Color32

    private Color epicColor = new Color(221f / 255f, 160f / 255f, 221f / 255f); // Light Purple
    public Camera playerCamera; // Assign your player camera via Inspector or in code
    public string targetTag = "YourTagHere"; // Replace with your object's tag to monitor
    // Keep track of objects currently in view to avoid repetitive logs
    private HashSet<GameObject> objectsInView = new HashSet<GameObject>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
         if (playerCamera == null)
            playerCamera = Camera.main;
    }

    void Update()
    {
        HandleInput();
        Animate();
        Kamera();
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

        if (!isFishing && !waitingForPopup)
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");

            if (x != 0) y = 0;
            input = new Vector2(x, y);

            if (input != Vector2.zero)
                lastMoveDirection = input;
        }

        if (isInCameraMode)
        {
            x = 0;
            y = 0;
            input = Vector2.zero;
            return;
        }

        if (isInFishingSpot && Input.GetMouseButtonDown(0) && !waitingForPopup)
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

        if (isFishing)
            anim.SetBool("isMoving", false);
    }

    void Kamera()
    {
        if (Input.GetKeyDown(KeyCode.C) &&
            GameObject.Find("PersistentObject2") == null &&
            GameObject.Find("InventoryMenu") == null)
        {
            isInCameraMode = !isInCameraMode;
            anim.SetBool("Camera", isInCameraMode);

            if (isInCameraMode)
            {
                FindAnyObjectByType<CameraMode>().SnapToPlayer();
            }

            if (cmeraSprite != null)
                cmeraSprite.enabled = isInCameraMode;

            if (cameraOverlay != null)
                cameraOverlay.SetActive(isInCameraMode);
        }
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

        if (isFishing)
        {
            if (fishingCoroutine == null)
                fishingCoroutine = StartCoroutine(FishingTimer());
        }
        else
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
                fishCaught = true;
                isFishing = false;
                fishingCoroutine = null;

                // Play popup animation and wait for it to finish
                waitingForPopup = true;
                if (popupAnimator != null)
                {
                    popupAnimator.SetTrigger("PlayPopUp");
                }

                yield return new WaitForSeconds(2f); // adjust to match animation duration
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
        if (pool.Count == 0) return null;

        return pool[Random.Range(0, pool.Count)];
    }

    void ShowFishPopup(FishData fish)
    {
        if (txtWindow != null && imgWindow != null && fishPanel != null)
        {
            txtWindow.text = fish.fishName;
            imgWindow.sprite = fish.fishSprite;
            fishPanel.SetActive(true);

            // Set the color of the text based on rarity
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
}
