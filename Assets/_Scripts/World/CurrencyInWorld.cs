using UnityEngine;

public class CurrencyInWorld : MonoBehaviour
{
    [SerializeField] private CurrencyType currencyType;
    [SerializeField] private int value = 1;
    
    public CurrencyType CurrencyType => currencyType;
    public int Value => value;

    private int index;
    public int Index => index;

    public void Init(int index)
    {
        this.index = index;
    }
}