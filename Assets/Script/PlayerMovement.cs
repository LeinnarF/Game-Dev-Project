using System;
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    public SpriteRenderer cmeraSprite;
    public GameObject cameraOverlay;

    public float speed = 2f;
    public float sprintSpeed = 4f;

    private float x;
    private float y;
    private Vector2 input;
    private bool isMoving;

    public bool isFishing = false;
    public bool isInFishingSpot = false;
    private FishingSpot currentFishingSpot;
    private Coroutine fishingCoroutine;
    private bool fishCaught = false;
    public bool isInCameraMode = false;

    // Adjustable raycast distance
    public float fishingRayDistance = 1.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
        Animate();
        Kamera();
        CheckForFishingSpot();
    }

    private void Animate()
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

    void Movement()
    {
        getInput();
        rb.linearVelocity = input * speed;
    }

    void getInput()
    {
        speed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? sprintSpeed : 2f;

        if (!isFishing)
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
        }

        if (isInCameraMode)
        {
            x = 0;
            y = 0;
            input = Vector2.zero;
            return;
        }

        // Prevent diagonal movement
        if (x != 0) y = 0;
        input = new Vector2(x, y);

        if (isInFishingSpot && Input.GetMouseButtonDown(0))
        {
            isFishing = !isFishing;

            if (isFishing)
            {
                Debug.Log("Fishing started");
                if (fishingCoroutine == null)
                    fishingCoroutine = StartCoroutine(FishingTimer());
            }
            else
            {
                Debug.Log("Fishing stopped");
                if (fishingCoroutine != null)
                {
                    StopCoroutine(fishingCoroutine);
                    fishingCoroutine = null;
                    Debug.Log("Fishing cancelled");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Game Closed");
        }
    }

    IEnumerator FishingTimer()
    {
        int waitTime = UnityEngine.Random.Range(3, 21);
        Debug.Log("Waiting for " + waitTime + " seconds to catch fish...");

        yield return new WaitForSeconds(waitTime);

        if (currentFishingSpot != null && isFishing)
        {
            string caught = currentFishingSpot.TryCatchFish();
            Debug.Log("You caught: " + caught);
            fishCaught = true;

            isFishing = false;
            fishingCoroutine = null;
        }
    }

    void Kamera()
    {
        if (Input.GetKeyDown(KeyCode.C) &&
            GameObject.Find("PersistentObject2") == null &&
            GameObject.Find("InventoryMenu") == null)
        {
            isInCameraMode = !isInCameraMode;
            Debug.Log("Camera mode toggled: " + isInCameraMode);
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

    // ✅ Always casts a blue ray downward (Vector2.down) to detect fishing spot
    void CheckForFishingSpot()
    {
        Vector2 direction = Vector2.down;
        Vector3 origin = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, fishingRayDistance, LayerMask.GetMask("Default"));

        if (hit.collider != null && hit.collider.CompareTag("FishingSpot"))
        {
            Debug.Log("Fishing spot detected: " + hit.collider.name);
            isInFishingSpot = true;
            currentFishingSpot = hit.collider.GetComponent<FishingSpot>();
        }
        else
        {
            isInFishingSpot = false;
            currentFishingSpot = null;
        }

        // Show ray in Scene view as BLUE
        Debug.DrawRay(origin, direction * fishingRayDistance, Color.blue);
    }
}
