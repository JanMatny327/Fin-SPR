using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Columns : MonoBehaviour
{
    public Item item;
    public Image itemIcon;

    private void Start()
    {
        ItemAlpha(0);
    }
    public void UpdateColumnUI()
    {
        itemIcon.sprite = item.itemImage;
        ItemAlpha(1);
        itemIcon.gameObject.SetActive(true);

    }
    public void RemoveColumn()
    {
        item = null;
        if (itemIcon != null)
        {
            itemIcon.gameObject.SetActive(false);
        }
    }
    public void ItemAlpha(float Icon)
    {
        Icon = Mathf.Clamp01(Icon);
        Color color = itemIcon.color;
        color.a = Icon;
        itemIcon.color = color;
    }
}
