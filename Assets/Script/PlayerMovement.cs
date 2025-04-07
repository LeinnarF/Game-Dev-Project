using System;
using UnityEngine;
using UnityEngine.Tilemaps;
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Animate();
        Kamera();
    }

    private void Animate()
    {
        if (input.magnitude > 0.1f || input.magnitude < -0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isMoving)
        {
            anim.SetFloat("x", x);
            anim.SetFloat("y", y);
        }
        anim.SetBool("isMoving", isMoving);

        anim.SetBool("isFishing", isFishing);

        if (isFishing) anim.SetBool("isMoving", false);
    }

    void Movement()
    {
        getInput();
        rb.linearVelocity = input * speed;
    }

    void getInput()
    {
        //sprint
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = 2f;
        }


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
        if (x != 0)
        {
            y = 0;
        }
        input = new Vector2(x, y);

        if (isInFishingSpot && Input.GetMouseButtonDown(0))
        {
            isFishing = !isFishing
            ;

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
        int waitTime = UnityEngine.Random.Range(3, 21); // 5 to 30 seconds
        Debug.Log("Waiting for " + waitTime + " seconds to catch fish...");

        yield return new WaitForSeconds(waitTime);

        if (currentFishingSpot != null && isFishing)
        {
            string caught = currentFishingSpot.TryCatchFish();
            Debug.Log("You caught: " + caught);
            fishCaught = true;

            // Auto stop fishing after catching
            isFishing = false;
            fishingCoroutine = null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FishingSpot"))
        {
            isInFishingSpot = true;
            currentFishingSpot = collision.GetComponent<FishingSpot>();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FishingSpot"))
        {
            isInFishingSpot = false;
            currentFishingSpot = null;
        }
    }

    void Kamera()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isInCameraMode = !isInCameraMode;
            Debug.Log("Camera mode toggled: " + isInCameraMode);
            anim.SetBool("Camera", isInCameraMode);

            if (isInCameraMode)
            {
                FindAnyObjectByType<CameraMode>().SnapToPlayer();
            }
            if (cmeraSprite != null)
            {
                cmeraSprite.enabled = isInCameraMode;
            }
            if (cameraOverlay != null)
            {
                cameraOverlay.SetActive(isInCameraMode);
            }
        }
    }
}
