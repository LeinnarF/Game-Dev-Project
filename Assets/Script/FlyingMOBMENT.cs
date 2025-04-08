using System.Numerics;
using System.Collections;
using UnityEditor.Timeline.Actions;
using UnityEngine;
public class FlyingMOBMENT : MonoBehaviour
{

    public float timer;
    public float dedTimer;
    float direction = 1;
    public float moveSpeed = 3;
    Rigidbody2D rb;
    private Camera mainCamera;
    private bool isDedInvoked = false;
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }
    
    // Update is called once per frame
    void Update()
    {  
       
        Animate();
        if (IsOutsideCamera())
        {
        isDedInvoked = true;
        InvokeRepeating(nameof(ded), dedTimer, dedTimer);
        }else if (IsInsideCamera() && isDedInvoked)
        {
            CancelInvoke(nameof(ded));
            isDedInvoked = false;
        }
    }

    void movement()
    {
         rb.linearVelocity = new ( rb.linearVelocity.x, rb.linearVelocity.y);
        int selection = Random.Range(0, 2);
        if (selection == 0)
        {
            rb.linearVelocity = new ( rb.linearVelocity.x, direction * moveSpeed);
        }else if (selection >= 1)
        {
            rb.linearVelocity = new (direction * moveSpeed, rb.linearVelocity.y);
        }
    }
    void ded()
    {
        Destroy(gameObject);
    }

    void flipDirection()
    {
        direction *= -1;
        if(direction > 0){
            transform.localScale = new (Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            
        }else if(direction < 0){
        transform.localScale = new (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

     private bool IsOutsideCamera()     
    {
        // Get the object's position in viewport coordinates
        UnityEngine.Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // Check if the object is outside the viewport
        return viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1;
    }
   private bool IsInsideCamera()
    {
        // Get the object's position in viewport coordinates
        UnityEngine.Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // Check if the object is inside the viewport
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1;
    }
    
  private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") )
        {
        InvokeRepeating(nameof(flipDirection), timer, timer);
        InvokeRepeating(nameof(movement), timer, timer);
        }
    }

     private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") )
        {
             CancelInvoke(nameof(flipDirection));
            CancelInvoke(nameof(movement));
           
        }
    }

     private void Animate()
    {
        if (rb.linearVelocity.magnitude > 0.1f || rb.linearVelocity.magnitude < -0.1f)
        {
            anim.SetBool("isMoving", true);
             
        }
        else if(rb.linearVelocity.magnitude == 0)
        {
            anim.SetBool("isMoving", false);
        }  
    }

    
}

