using UnityEngine;

public class BehindObject : MonoBehaviour
{
    bool isBehindObject = false;
    SpriteRenderer sr;
    Color color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBehindObject)
        {
            sr.sortingOrder = 3;
            color = sr.color;
            color.a = 0.75f; 
            sr.color = color;
        }
        else
        {
            sr.sortingOrder = 1;
            color = sr.color;
            color.a = 1f;
            sr.color = color;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isBehindObject = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isBehindObject = false;
        }
    }
}
