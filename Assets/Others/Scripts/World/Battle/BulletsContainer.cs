using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Utility;
using TheSTAR.Sound;
using Zenject;

public class BulletsContainer : MonoBehaviour
{
    [SerializeField] private UnityDictionary<BulletType, Bullet> bulletPrefabs;

    private SoundController sounds;

    private List<Bullet> _activeBullets = new();
    private Dictionary<BulletType, List<Bullet>> _bulletsPool;

    private const float BulletsSpeed = 8;

    [Inject]
    private void Construct(SoundController sounds)
    {
        this.sounds = sounds;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        var allBulletTypes = EnumUtility.GetValues<BulletType>();
        _bulletsPool = new();
        foreach (var bulletType in allBulletTypes) _bulletsPool.Add(bulletType, new());
    }

    public void Shoot(Shooter shooter, BulletType bulletType, int force, Vector3 direction)
    {
        Bullet bullet = PoolUtility.GetPoolObject(_bulletsPool[bulletType], info => !info.gameObject.activeSelf, shooter.ShootingPos.position, CreateNewBullet);
        bullet.transform.LookAt(shooter.ShootingPos.position + direction);
        bullet.Init(BulletsSpeed, force);
        _activeBullets.Add(bullet);

        Bullet CreateNewBullet(Vector3 pos)
        {
            var bullet = Instantiate(bulletPrefabs.Get(bulletType), shooter.ShootingPos.position, Quaternion.identity, transform);
            _bulletsPool[bulletType].Add(bullet);
            bullet.OnCompleteFlyEvent += OnBulletCompleteFly;
            return bullet;
        }
    }

    #region Simulation

    public void StopSimulate()
    {
        foreach (var pool in _bulletsPool.Values)
        {
            foreach (var b in pool) b.gameObject.SetActive(false);
        }
        _activeBullets.Clear();
    }

    private void Update()
    {
        for (int i = 0; i < _activeBullets.Count; i++)
        {
            Bullet b = _activeBullets[i];
            b.Fly();
        }
    }

    #endregion

    private void OnBulletCompleteFly(Bullet b)
    {
        _activeBullets.Remove(b);
    }
}

public enum BulletType
{
    Default
}