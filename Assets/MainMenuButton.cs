using System.Collections;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    public static GameObject persistentObject3;
    public static GameObject persistentObject2;
    public static GameObject cameraOverlay;
    private GameObject CameraOverlay; // Camera overlay GameObject

    private bool isInventoryActive = false;
    private bool isLogbookActive = false;
    private bool isCameraActive = false;

    void Start()
    {
        StartCoroutine(FindUIElements());
    }

    IEnumerator FindUIElements()
    {
        yield return new WaitForSeconds(0.1f);

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        // Find Inventory GameObject (parent of InventoryMenu with tag "Inventory")
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "PersistentObject3")
            {
                persistentObject3 = obj;
                if (persistentObject3 != null)
                {
                    Debug.Log("Inventory found: " + persistentObject3.name);
                    break;
                }
            }
        }

        // Find PersistentObject2
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "PersistentObject2")
            {
                persistentObject2 = obj;
                Debug.Log("PersistentObject2 found.");
                break;
            }
        }

        // Find CameraOverlay
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "CameraOverlay")
            {
                cameraOverlay = obj;
                Debug.Log("CameraOverlay found.");
                break;
            }
        }
    }

    public void BackpackM()
    {
        Debug.Log("Backpack button clicked.");

        bool isBlockedBag = (cameraOverlay != null && cameraOverlay.activeInHierarchy)
                          || (persistentObject2 != null && persistentObject2.activeInHierarchy);

        if (!isBlockedBag)
        {
            isInventoryActive = !isInventoryActive;

            if (persistentObject3 != null)
            {
                persistentObject3.SetActive(isInventoryActive);
                Debug.Log("Inventory toggled: " + isInventoryActive);
            }
            else
            {
                Debug.LogWarning("Inventory GameObject not found or not yet loaded.");
            }
        }
        else
        {
            Debug.Log("Backpack UI blocked by another UI.");
        }
    }

    public void LogbookM()
    {
        Debug.Log("Logbook button clicked.");

        bool isBlockedLogbook = (cameraOverlay != null && cameraOverlay.activeInHierarchy)
                              || (persistentObject3 != null && persistentObject3.activeInHierarchy);

        if (!isBlockedLogbook)
        {
            isLogbookActive = !isLogbookActive;

            if (persistentObject2 != null)
            {
                persistentObject2.SetActive(isLogbookActive);
                Debug.Log("Logbook toggled: " + isLogbookActive);
            }
            else
            {
                Debug.LogWarning("PersistentObject2 not found or not yet loaded.");
            }
        }
        else
        {
            Debug.Log("Logbook UI blocked by another UI.");
        }
    }

    public void CameraM()
    {
        Debug.Log("Camera button clicked.");

        // Check if the camera overlay is active
        if (cameraOverlay != null)
        {
            isCameraActive = !isCameraActive; // Toggle camera state
            cameraOverlay.SetActive(isCameraActive); // Set camera overlay active/inactive

            // Optionally, you can also manage the player state or other UI elements here
            Debug.Log("Camera overlay toggled: " + isCameraActive);
        }
        else
        {
            Debug.LogWarning("CameraOverlay not found or not yet loaded.");
        }
    }

    public void ChatM()
    {
        Debug.Log("Chat button clicked.");
        // Add chat logic here if needed
    }
}
