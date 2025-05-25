using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    //ItemData
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;

    [SerializeField]
    private int maxNumberOfItems;

    //ItemSlot
    [SerializeField]
    private Text quantityText;

    [SerializeField]
    private Image itemImage;

    //Item DescriptionSlot
    public Image itemDescriptionImage;
    public Text ItemDescriptionNameText;
    public Text ItemDescriptionText;

    public GameObject InventoryMenu;

    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

   private void Start()
{
    StartCoroutine(FindUIElements());
}

IEnumerator FindUIElements()
{
    yield return new WaitForSeconds(0.1f);

    GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

    foreach (GameObject obj in allObjects)
    {
        if (obj.name == "InventoryMenu")
        {
            Transform parent = obj.transform.parent;
            if (parent != null)
            {
                InventoryMenu = parent.gameObject;
                inventoryManager = InventoryMenu.GetComponent<InventoryManager>(); // ✅ FIXED
                Debug.Log("Inventory found: " + InventoryMenu.name);
                break;
            }
        }
    }
}

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        //check to see if the slot is already full
        if (isFull)
            return quantity;

        if (this.itemName != "" && this.itemName != itemName)
            return quantity;

        //Update Name
        this.itemName = itemName;

        //Update Image
        this.itemSprite = itemSprite;
        itemImage.sprite = itemSprite;

        //Update Desription
        this.itemDescription = itemDescription;

        //Update Quantity
        this.quantity += quantity;
        if (this.quantity >= maxNumberOfItems)
        {
            quantityText.text = maxNumberOfItems.ToString();
            quantityText.enabled = true;
            isFull = true;

            //Return the LeftOvers
            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;

            return extraItems;
        }

        //Update Quantity Text
        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        return 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
{
    if (thisItemSelected)
    {
        // Show item info first
        ItemDescriptionNameText.text = itemName;
        ItemDescriptionText.text = itemDescription;
        itemDescriptionImage.sprite = itemSprite != null ? itemSprite : emptySprite;

        bool usable = inventoryManager.UseItem(itemName);
        if (usable)
        {
            this.quantity -= 1;
            quantityText.text = this.quantity.ToString();
            if (this.quantity <= 0)
                EmptySlot();
        }
    }
    else
    {
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;

        ItemDescriptionNameText.text = itemName;
        ItemDescriptionText.text = itemDescription;
        itemDescriptionImage.sprite = itemSprite != null ? itemSprite : emptySprite;
    }
}

    private void EmptySlot()
{
    quantity = 0;
    isFull = false;
    itemName = "";
    itemDescription = "";
    itemSprite = null;

    quantityText.enabled = false;
    itemImage.sprite = emptySprite;

    ItemDescriptionNameText.text = "";
    ItemDescriptionText.text = "";
    itemDescriptionImage.sprite = emptySprite;

    thisItemSelected = false;
    selectedShader.SetActive(false);
}

    public void OnRightClick()
    {

    }
}
