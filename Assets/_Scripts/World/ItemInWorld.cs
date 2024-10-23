using UnityEngine;

public class ItemInWorld : MonoBehaviour
{
    [SerializeField] private ItemInWorldType itemType;
    [SerializeField] private int value = 1;
    
    public ItemInWorldType ItemType => itemType;
    public int Value => value;

    private int index;
    public int Index => index;

    public void Init(int index)
    {
        this.index = index;
    }
}

public enum ItemInWorldType
{
    Coin,
    HP
}