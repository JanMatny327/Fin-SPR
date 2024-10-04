using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUi : MonoBehaviour
{
    Inventory inven;

    public GameObject InventoryPanel;
    bool activelnventory = false;

    public Columns[] columns;
    public Transform colHolder;

    private void Start()
    {
        inven = Inventory.Instance;
        columns = colHolder.GetComponentsInChildren<Columns>();
        inven.oncolumnCountChange += ColumnChange;
        inven.onChangeltem += RedrawColumnUI;
        InventoryPanel.SetActive(activelnventory);
    }
    private void ColumnChange(int val)
    {
        for (int i = 0; i < columns.Length; i++)
        {
            if (i < inven.ColumnCnt)
                columns[i].GetComponent<Button>().interactable = true;
            else
                columns[i].GetComponent<Button>().interactable = false;
        } 
    }   
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            activelnventory = !activelnventory;
            InventoryPanel.SetActive(activelnventory);
        }
    }

    public void AddColumn()
    {
        inven.ColumnCnt++;
    }

    void RedrawColumnUI()
    {
        for (int i = 0;i < columns.Length; i++)
        {
            columns[i].RemoveColumn();
        }
        for (int i = 0; i < columns.Length; i++)
        {
            if (i < inven.items.Count) // 리스트의 크기 체크
            {
                columns[i].item = inven.items[i];
                columns[i].UpdateColumnUI();
            }
            else
            {
                columns[i].RemoveColumn(); // 아이템이 없으면 열 비우기
            }
        }
    }

}
