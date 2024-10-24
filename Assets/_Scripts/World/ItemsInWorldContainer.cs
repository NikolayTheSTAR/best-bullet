using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Data;
using Zenject;

public class ItemsInWorldContainer : MonoBehaviour
{
    [SerializeField] private ItemInWorld[] items;

    private DataController data;

    [Inject]
    private void Construct(DataController data)
    {
        this.data = data;
    }

    private void Start()
    {
        LoadCurrencyItems();
    }

    public void LoadCurrencyItems()
    {
        var collectedCurrencyItems = data.gameData.levelData.collectedItems;

        for (int i = 0; i < items.Length; i++)
        {
            items[i].Init(i);
            items[i].gameObject.SetActive(!collectedCurrencyItems.ContainsKey(i) || !collectedCurrencyItems[i]);
        }
    }
}