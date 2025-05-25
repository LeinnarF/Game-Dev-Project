using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;

    public bool UseItem()
{
    GameObject playerObj = GameObject.Find("Player");
    if (playerObj == null)
    {
        Debug.LogWarning("Player object not found when trying to use item.");
        return false;
    }

    PlayerStamina playerStamina = playerObj.GetComponent<PlayerStamina>();
    if (playerStamina == null)
    {
        Debug.LogWarning("PlayerStamina component not found on Player.");
        return false;
    }

    if (statToChange == StatToChange.stamina)
    {
        if (playerStamina.stamina == playerStamina.maxStamina)
        {
            return false;
        }

        playerStamina.ChangeStamina(amountToChangeStat);
        return true;
    }

    return false;
}
    
    public enum StatToChange
    {
        stamina
    };
}
