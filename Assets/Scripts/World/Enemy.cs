using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    [SerializeField] private Shooter shooter;

    public void Init(BulletsContainer bullets, int currentHP, int maxHP)
    {
        hpSystem.Init(currentHP, maxHP);
        shooter.Init(bullets, BulletType.Default, 1);
    }

    private void Update()
    {
        MoveTo(Vector2.zero);
    }
}