using System;
using UnityEngine;
using Zenject;
using TheSTAR.Data;
using TheSTAR.Sound;
using TheSTAR.GUI;
using TheSTAR.Utility;

public class GameWorld : MonoBehaviour
{
    [SerializeField] private CurrencyInWorld[] currency;

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
        var collectedCurrencyItems = data.gameData.levelData.collectedCurrencyItems;

        for (int i = 0; i < currency.Length; i++)
        {
            currency[i].Init(i);
            currency[i].gameObject.SetActive(!collectedCurrencyItems.ContainsKey(i) || !collectedCurrencyItems[i]);
        }
    }
}