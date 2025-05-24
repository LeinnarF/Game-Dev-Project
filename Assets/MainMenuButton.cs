using System.Collections;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    public static GameObject Inventory;
    public static GameObject persistentObject2;
    public static GameObject cameraOverlay;
    private bool isInventoryActive = false;
    private bool isLogbookActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the Inventory GameObject
        Inventory = GameObject.Find("InventoryMenu");
        // Find the persistent object
        persistentObject2 = GameObject.Find("PersistentObject2");
        // Find the CameraOverlay GameObject
        cameraOverlay = GameObject.Find("Camera");
        StartCoroutine(FindLogbookAndImages());
    }
    IEnumerator FindLogbookAndImages()
    {
        yield return new WaitForSeconds(0.1f);

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "PersistentObject2")
            {
                persistentObject2 = obj;
                break;
            }
        }
    }
    public void BackpackM()
    {
        // Logic to open the backpack
        Debug.Log("Backpack button clicked.");
        bool isBlockedBag = (cameraOverlay != null && cameraOverlay.activeInHierarchy) || (persistentObject2 != null && persistentObject2.activeInHierarchy);

        if (!isBlockedBag)
        {
            Debug.Log("Button key pressed. Toggling Inventory.");
            isInventoryActive = !isInventoryActive;
            if (Inventory == null)
            {
                Inventory.SetActive(isInventoryActive);
            }
        }
    }

    public void CameraM()
    {
        // Logic to open the camera
        Debug.Log("Camera button clicked.");
        // Add camera logic here if needed
    }

    public void LogbookM()
    {
        Debug.Log("Logbook button clicked.");
        bool isBlockedLogbook = (cameraOverlay != null && cameraOverlay.activeInHierarchy) || (Inventory != null && Inventory.activeInHierarchy);

        if (!isBlockedLogbook)
        {
            Debug.Log("Button key pressed. Toggling PersistentObject2.");
            isLogbookActive = !isLogbookActive;
            if (persistentObject2 == null)
            {
                Debug.LogWarning("PersistentObject2 found. Cannot toggle logbook.");
                persistentObject2.SetActive(isLogbookActive);
            }
            else
            { persistentObject2.SetActive(isLogbookActive);}
        }
    }

    public void ChatM()
    {
        // Logic to open the chat
        Debug.Log("Chat button clicked.");
        // Add chat logic here if needed
    }
}
