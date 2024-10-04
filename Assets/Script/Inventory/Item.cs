using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public enum ItemType
{
    Hp,
    ATK,
    DEX,
    Healing
}

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public int itemStat;


    public bool Use()
    {
        bool isUsed =false;
        isUsed = true;
        return isUsed;
    }
}
