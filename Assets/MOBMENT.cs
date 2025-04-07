using System.Numerics;
using System.Collections;
using UnityEditor.Timeline.Actions;
using UnityEngine;
public class WalkingMOB : MonoBehaviour
{

    public float timer;
    public float dedTimer;
    float direction = 1;
    public float moveSpeed = 3;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating(nameof(flipDirection), timer, timer);
        InvokeRepeating(nameof(ded), dedTimer, dedTimer);
    }
    
    // Update is called once per frame
    void Update()
    {  
        rb.linearVelocity = new (direction * moveSpeed, rb.linearVelocity.y);
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
    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if(collision.gameObject.CompareTag("Mob Collider"))
    //     {
    //         flipDirection();
    //     }
        
        
    // }
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("edge") || collision.gameObject.CompareTag("Tree"))
    //     {
    //         flipDirection();
            
    //     }
    // }
    
  
}

