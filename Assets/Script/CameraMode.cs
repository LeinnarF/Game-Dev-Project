using Unity.VisualScripting;
using UnityEngine;

public class CameraMode : MonoBehaviour
{

    public float moveSpeed = 5f;
    private PlayerMovement player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (player.isInCameraMode)
            {
                Vector3 move = new Vector3(
                    Input.GetAxisRaw("Horizontal"),
                    Input.GetAxisRaw("Vertical"),
                    0f
                );
                transform.position += move.normalized * moveSpeed * Time.deltaTime;
            }
        }
       
    }
    public void SnapToPlayer()
    {
        if (player != null)
        {
            Vector3 newPos = player.transform.position;
            newPos.z = transform.position.z; // maintain camera z-position
            transform.position = newPos;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Animal")||other.CompareTag("Flying Animal"))
        {
            Debug.Log("Animal entered trigger zone!");
            if (Input.GetMouseButtonDown(0)) // Detects left mouse button click
            {
                    Debug.Log("Clicked on the object!");
                    Destroy(other.gameObject);
                
            }
        }
        }
    

}
