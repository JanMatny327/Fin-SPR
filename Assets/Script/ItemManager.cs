using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ItemData
{
    public static int itemDamageData = 0;
    public static int itemHpData = 0;
    public static int itemArmorData = 0;
    public static int itemSpeedData = 0;
    public static int itemAttackSpeedData = 0;
    public static float itemCriticalPercentData = 0;
    public static float itemCriticalDamageData = 0;
}

public class ItemManager : MonoBehaviour
{
    public string itemName;

    [Header("아이템 성능 관리")]
    public int itemDamage;
    public int itemHp;
    public int itemArmor;
    public int itemSpeed;
    public int itemAttackSpeed;
    public float itemCriticalPercent;
    public float itemCriticalDamage;

    [Header("아이템 등급 관리")]
    public ItemRating itemRating;

    public enum ItemRating
    {
        unCommon,
        Common,
        Rare,
        Uniqe,
        Lengendray
    }

    [Header("아이템 설명")]
    public string itemStatDescription = 
        "공격력 : " + ItemData.itemDamageData +
        "\n 체력 : " + ItemData.itemHpData +
        "\n 방어력 : " + ItemData.itemArmorData +
        "\n 이동속도 : " + ItemData.itemSpeedData +
        "\n 공격속도 : " + ItemData.itemAttackSpeedData +
        "\n 크리티컬 확률 : " + ItemData.itemCriticalPercentData +
        "\n 크리티컬 데미지 : " + ItemData.itemCriticalDamageData;
    public string itemDescription; // 아이템 설명

    private void Update()
    {
        itemStatDescription = 
        "공격력 : " + ItemData.itemDamageData +
        "\n 체력 : " + ItemData.itemHpData +
        "\n 방어력 : " + ItemData.itemArmorData +
        "\n 이동속도 : " + ItemData.itemSpeedData +
        "\n 공격속도 : " + ItemData.itemAttackSpeedData +
        "\n 크리티컬 확률 : " + ItemData.itemCriticalPercentData +
        "\n 크리티컬 데미지 : " + ItemData.itemCriticalDamageData;

        ItemData.itemDamageData = itemDamage;
        ItemData.itemHpData = itemHp;
        ItemData.itemArmorData = itemArmor;
        ItemData.itemSpeedData = itemSpeed;
        ItemData.itemAttackSpeedData = itemAttackSpeed;
        ItemData.itemCriticalPercentData = itemCriticalPercent;
        ItemData.itemCriticalDamageData = itemCriticalDamage;
    }
}
