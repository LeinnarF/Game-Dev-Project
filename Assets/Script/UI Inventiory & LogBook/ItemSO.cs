using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;

    public AttributesToChange attributesToChange = new AttributesToChange();
    public int amountToChangeAttribute;

    public enum StatToChange
    {
        none,
        health,
        mana,
        stamina
    };

    public enum AttributesToChange
    {
        none,
        strength,
        defense,
        intelligence,
        agility
    };
}
