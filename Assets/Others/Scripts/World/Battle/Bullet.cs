using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletType bulletType;
    [SerializeField] private Transform visual;

    public event Action<Bullet> OnCompleteFlyEvent; // todo use
    
    private float speed;

    private int force;
    private DateTime endLifeTime;
    public DateTime EndLifeTime => endLifeTime;

    // todo можно чтобы пуля уничтожалась если врезается в любой объект (слой должен быть заметен для слоя Bullet)
    // но урон соответственно наносится только сущностям с HpSystem

    // todo отдельный слой для пуль

    public void Init(float speed, int force, int maxLifetimeSeconds)
    {
        this.speed = speed;
        this.force = force;
        endLifeTime = DateTime.Now.AddSeconds(maxLifetimeSeconds);
    }

    public void Fly()
    {
        transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
    }

    public void ForceDespawn()
    {
        CompleteFly();
    }

    private void CompleteFly()
    {
        gameObject.SetActive(false);
        OnCompleteFlyEvent?.Invoke(this);
    }
}