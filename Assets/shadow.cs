using UnityEngine;

public class ShadowController : MonoBehaviour
{
    public GameObject shadow; // Assign the shadow GameObject in the Inspector
    public float shadowOffsetY = -3f; // Offset for the shadow's Y position

    void Update()
    {
        if (shadow != null)
        {
            // Get the current position of the owl
            Vector3 owlPosition = transform.position;

            // Update the shadow's position
            shadow.transform.position = new Vector3(owlPosition.x, owlPosition.y + shadowOffsetY, owlPosition.z);
        }
    }
}