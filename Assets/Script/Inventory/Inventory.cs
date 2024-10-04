using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    public delegate void OnColumnCountChange(int val);
    public OnColumnCountChange oncolumnCountChange;

    public delegate void OnChangeltem();
    public OnChangeltem onChangeltem;

    public List<Item> items = new List<Item>();

    private int columnCnt;
    public int ColumnCnt
    {
        get => columnCnt;
        set
        {
            columnCnt = value;
            oncolumnCountChange.Invoke(columnCnt);
        }
    }

    void Start()
    {
        ColumnCnt = 12;
    }

    public bool AddItem(Item _item)
    {
        if (items.Count < ColumnCnt)
        {
            items.Add(_item);
            onChangeltem.Invoke();
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FieldItem"))
        {
            FieldItems fielditem =  collision.GetComponent<FieldItems>();
            if (AddItem(fielditem.GetItem()))
            {
                fielditem.Destroyitem();
            }
        }
    }
}
