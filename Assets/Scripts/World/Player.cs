using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Player : MonoBehaviour, ICameraFocusable, IKeyInputHandler
{
    [SerializeField] private NavMeshAgent meshAgent;
    [SerializeField] private Transform visualTran;
    [SerializeField] private CurrencyInWorldGetter currencyGetter;

    private CurrencyController currency;

    [Inject]
    private void Construct(CurrencyController currency)
    {
        this.currency = currency;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        currencyGetter.Init();
        currencyGetter.OnGetCurrencyEvent += (currencyType, count) => currency.AddCurrency(currencyType, count, true);
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