using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogBookMenu : MonoBehaviour
{
    public GameObject LogbookMenu;
    private bool menuActivated;
   // Reference to the InventoryUI script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("LogBook") && menuActivated)
            {
         
            Time.timeScale = 1;
            LogbookMenu.SetActive(false);
            menuActivated = false;
            }

        else if (Input.GetButtonDown("LogBook") && !menuActivated && GameObject.Find("InventoryMenu") == null && GameObject.Find("CameraOverlay") == null)
        {
       
            Time.timeScale = 0;
            LogbookMenu.SetActive(true);
            menuActivated = true;
    }
    }
}
