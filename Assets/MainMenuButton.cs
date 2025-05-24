using System.Collections;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    public static GameObject InventoryMenu;
    public static GameObject persistentObject2;
    public static GameObject cameraOverlay;

    private bool isInventoryActive = false;
    private bool isLogbookActive = false;

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
            if (obj.name == "InventoryMenu")
            {
                Transform parent = obj.transform.parent;
                if (parent != null)
                {
                    InventoryMenu = parent.gameObject;
                    Debug.Log("Inventory found: " + InventoryMenu.name);
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

            if (InventoryMenu != null)
            {
                InventoryMenu.SetActive(isInventoryActive);
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
                              || (InventoryMenu != null && InventoryMenu.activeInHierarchy);

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
        // Add camera toggle logic here if needed
    }

    public void ChatM()
    {
        Debug.Log("Chat button clicked.");
        // Add chat logic here if needed
    }
}
