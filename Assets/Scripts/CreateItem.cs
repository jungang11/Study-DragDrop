using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateItem : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject[] inventorySlots;
    [SerializeField] GameObject item;

    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
    }

    private void Start()
    {
        btn.onClick.AddListener(() => { CreateNewItem(); });
    }

    private void CreateNewItem()
    {
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            if(inventorySlots[i].transform.childCount == 0)
            {
                // RectTransform slotTransform = inventorySlots[i].GetComponent<RectTransform>();
                GameObject newItem = Instantiate(item, new Vector2(0f, 0f), Quaternion.identity);
                newItem.transform.SetParent(inventorySlots[i].transform, false);
                break;
            }
        }
    }
}
