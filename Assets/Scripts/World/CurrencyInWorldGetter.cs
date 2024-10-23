using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

public class CurrencyInWorldGetter : MonoBehaviour
{
    [SerializeField] private EntranceTrigger trigger;
    [SerializeField] private Transform flyToPos;

    public event Action<int, CurrencyType, int> OnGetCurrencyEvent;

    public void Init()
    {
        trigger.Init(OnEnter, null);
    }

    private void OnEnter(Collider col)
    {
        if (col.CompareTag("CurrencyInWorld"))
        {
            GetCurrencyFromWorld(col.gameObject.GetComponent<CurrencyInWorld>());
        }
    }

    private void GetCurrencyFromWorld(CurrencyInWorld currency)
    {
        currency.gameObject.SetActive(false);
        // todo fly currency to flyToPos (with DropContainer)
        // todo after fly call OnGetCurrencyEvent
        // todo зафиксировать что монета сбрана, она не должна появляться в следующей сессии

        OnGetCurrencyEvent?.Invoke(currency.Index, currency.CurrencyType, currency.Value);
    }
}