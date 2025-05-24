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
        if (statToChange == StatToChange.stamina)
        {
            PlayerStamina playerStamina = GameObject.Find("Player").GetComponent<PlayerStamina>();
            if (playerStamina.stamina == playerStamina.maxStamina)
            {
                return false;
            }
            else
            {
                playerStamina.ChangeStamina(amountToChangeStat);
                return true;
            }
        }
        return false;
    }
    

    public enum StatToChange
    {
        stamina
    };
}
