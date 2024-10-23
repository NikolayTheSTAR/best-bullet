using System;
using TheSTAR.Data;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Player : MonoBehaviour, ICameraFocusable, IKeyInputHandler
{
    [SerializeField] private NavMeshAgent meshAgent;
    [SerializeField] private Transform visualTran;
    [SerializeField] private CurrencyInWorldGetter currencyGetter;

    private DataController data;
    private CurrencyController currency;

    [Inject]
    private void Construct(DataController data, CurrencyController currency)
    {
        this.data = data;
        this.currency = currency;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        currencyGetter.Init();
        currencyGetter.OnGetCurrencyEvent += (index, currencyType, count) =>
        {
            currency.AddCurrency(currencyType, count);
            var colledtedItems = data.gameData.levelData.collectedCurrencyItems;
            if (!colledtedItems.ContainsKey(index)) colledtedItems.Add(index, true);
            else colledtedItems[index] = true;
        };
    }

    #region KeyInput

    public void OnStartKeyInput() 
    {}

    public void KeyInput(Vector2 direction)
    {
        MoveTo(direction);
    }

    public void OnEndKeyInput() 
    {}

    #endregion

    private void MoveTo(Vector2 input)
    {
        Vector3 finalMoveDirection;

        if (input.x != 0 || input.y != 0)
        {
            var tempMoveDirection = new Vector3(input.x, 0, input.y);
            finalMoveDirection = transform.position + tempMoveDirection;
        
            // rotate

            var lookRotation = Quaternion.LookRotation(tempMoveDirection);
            var euler = lookRotation.eulerAngles;
            visualTran.rotation = Quaternion.Euler(0, euler.y, 0);
        }
        else
        {
            finalMoveDirection = transform.position;            
        }

        meshAgent.SetDestination(finalMoveDirection);
    }
}