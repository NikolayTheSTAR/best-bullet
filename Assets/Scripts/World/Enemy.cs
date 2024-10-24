using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    [SerializeField] private Shooter shooter;

    private const float RotationSteed = 1;

    public void Init(BulletsContainer bullets, int currentHP, int maxHP)
    {
        hpSystem.Init(currentHP, maxHP);
        shooter.Init(bullets, BulletType.Default, 1);
    }

    public void Simulate(Transform lookAt)
    {
        MoveTo(Vector2.zero);
        SmoothLookAt(lookAt);
    }

    private void SmoothLookAt(Transform target)
    {
        // Получаем направление к игроку
        Vector3 direction = target.position - visualTran.position;
        
        // Проектируем направление на плоскость XZ, убирая компонент Y
        direction.y = 0;

        // Если направление не нулевое, разворачиваем врага
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            visualTran.rotation = Quaternion.Slerp(visualTran.rotation, lookRotation, Time.deltaTime * RotationSteed);
        }
    }
}