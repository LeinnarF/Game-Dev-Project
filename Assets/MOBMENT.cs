using System.Numerics;
using UnityEngine;
public class WalkingMOB : MonoBehaviour
{


    float direction = 1;
    public float moveSpeed = 3;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {  
        rb.linearVelocity = new (direction * moveSpeed, rb.linearVelocity.y);
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Mob Collider"))
        {
            flipDirection();
        }
        
        
    }
    
}

