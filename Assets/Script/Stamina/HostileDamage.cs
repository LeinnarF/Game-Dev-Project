using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileDamage : MonoBehaviour
{
    public int damage = 2;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStamina playerStamina = collision.gameObject.GetComponent<PlayerStamina>();
            if (playerStamina != null)
            {
                playerStamina.TakeDamage(damage);
            }
        }
    }
}
