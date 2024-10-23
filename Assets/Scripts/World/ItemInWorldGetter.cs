using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

public class ItemInWorldGetter : MonoBehaviour
{
    [SerializeField] private EntranceTrigger trigger;
    //[SerializeField] private Transform flyToPos;

    public event Action<int, ItemInWorldType, int> OnGetCurrencyEvent;

    public void Init()
    {
        trigger.Init(OnEnter, null);
    }

    private void OnEnter(Collider col)
    {
        if (col.CompareTag("Item"))
        {
            GetItemFromWorld(col.gameObject.GetComponent<ItemInWorld>());
        }
    }

    private void GetItemFromWorld(ItemInWorld item)
    {
        item.gameObject.SetActive(false);
        // todo fly currency to flyToPos (with DropContainer)
        // todo after fly call OnGetCurrencyEvent
        // todo зафиксировать что монета сбрана, она не должна появляться в следующей сессии

        OnGetCurrencyEvent?.Invoke(item.Index, item.ItemType, item.Value);
    }
}
