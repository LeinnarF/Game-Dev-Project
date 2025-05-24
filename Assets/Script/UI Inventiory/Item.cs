using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite sprite;

    [TextArea]
    [SerializeField]
    private string itemDescription;

    private InventoryManager inventoryManager;

    public static GameObject InventoryMenu;

    private bool initialized = false;

    void Start()
    {
        StartCoroutine(FindUIElements());
    }

    IEnumerator FindUIElements()
    {
        yield return new WaitForSeconds(0.1f);

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "PersistentObject3")
            {
                Transform parent = obj.transform.parent;
                if (parent != null)
                {
                    InventoryMenu = parent.gameObject;
                    inventoryManager = InventoryMenu.GetComponent<InventoryManager>();
                    if (inventoryManager != null)
                    {
                        initialized = true;
                        Debug.Log("InventoryManager found.");
                    }
                    else
                    {
                        Debug.LogWarning("InventoryManager component not found on InventoryMenu.");
                    }
                    break;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!initialized || inventoryManager == null)
            {
                Debug.LogWarning("InventoryManager not ready yet. Skipping item pickup.");
                return;
            }

            int leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription);
            if (leftOverItems <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                quantity = leftOverItems;
            }
        }
    }
}
