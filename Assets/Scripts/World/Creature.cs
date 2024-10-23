using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    [SerializeField] protected HpSystem hpSystem;

    public HpSystem HpSystem => hpSystem;
}