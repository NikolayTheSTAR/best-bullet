using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Transform shootingPos;
    public Transform ShootingPos => shootingPos;

    private BulletType bulletType;
    private int force;

    private BulletsContainer bullets;

    [Inject]
    private void Construct(BulletsContainer bullets)
    {
        this.bullets = bullets;
    }

    public void Init(BulletType bulletType, int force)
    {
        this.bulletType = bulletType;
        this.force = force;
    }

    public void Shoot(Vector3 direction)
    {
        bullets.Shoot(this, bulletType, force, direction);
    }
}