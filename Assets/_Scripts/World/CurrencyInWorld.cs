using UnityEngine;

public class CurrencyInWorld : MonoBehaviour
{
    [SerializeField] private CurrencyType currencyType;
    [SerializeField] private int value = 1;
    
    public CurrencyType CurrencyType => currencyType;
    public int Value => value;
}