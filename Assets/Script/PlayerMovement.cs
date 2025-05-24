using System.Collections;
using UnityEngine;

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

    public float fishingRayDistance = 1.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInput();
        Animate();
        Kamera();
        CheckForFishingSpot();

        // Cancel fishing if player starts moving
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

        if (!isFishing)
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");

            if (x != 0) y = 0; // Prevent diagonal movement
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

        if (isInFishingSpot && Input.GetMouseButtonDown(0))
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
        Color rayColor = Color.blue;

        if (hit.collider != null && hit.collider.CompareTag("FishingSpot"))
        {
            isInFishingSpot = true;
            currentFishingSpot = hit.collider.GetComponent<FishingSpot>();
            rayColor = Color.cyan;
        }
        else
        {
            isInFishingSpot = false;
            currentFishingSpot = null;
        }

        Debug.DrawRay(origin, direction * fishingRayDistance, rayColor);
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
            string caught = currentFishingSpot.TryCatchFish();
            fishCaught = true;

            isFishing = false;
            fishingCoroutine = null;
        }
    }
}
