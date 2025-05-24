
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileDamage : MonoBehaviour
{
    public PlayerStamina playerStamina;
    public int damage = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerStamina.TakeDamage(damage);
        }
    }
}
