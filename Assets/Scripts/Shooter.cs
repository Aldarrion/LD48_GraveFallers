using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootDirection
{
    Right,
    Up,
    Left,
    Down,
}

public class Shooter : MonoBehaviour
{
    public GameObject Projectile;
    public Transform ProjectileSpawnPoint;
    public ShootDirection Direction;
    public float Speed;
    public float Cooldown;
    public int WarmupProjectileCount = 10;
    private float _currentCooldown;

    private float GetRotation()
    {
        return (int)Direction * 90;
    }

    private void Start()
    {
        _currentCooldown = Cooldown;
        transform.eulerAngles = new Vector3(0, 0, GetRotation());

        float projectileOffset = Cooldown * Speed;
        Vector3 direction = GetDirection();
        for (int i = 0; i < WarmupProjectileCount; ++i)
        {
            SpawnProjectile(direction * projectileOffset * i);
        }
    }

    private Vector2 GetDirection()
    {
        switch (Direction)
        {
            case ShootDirection.Left:
                return Vector2.left;
            case ShootDirection.Right:
                return Vector2.right;
            case ShootDirection.Up:
                return Vector2.up;
            case ShootDirection.Down:
                return Vector2.down;
            default: 
                return Vector2.zero;
        }
    }

    private void SpawnProjectile(Vector3 offset)
    {
        GameObject projectileObject = Instantiate(Projectile, ProjectileSpawnPoint.position + offset, Quaternion.identity);

        var projectile = projectileObject.GetComponent<Projectile>();
        projectile.transform.eulerAngles = new Vector3(0, 0, GetRotation());
        projectile.Direction = GetDirection();
        projectile.Speed = Speed;
    }

    void Update()
    {
        _currentCooldown -= Time.deltaTime;
        if (_currentCooldown <= 0)
        {
            _currentCooldown = Cooldown + Random.Range(-0.1f, 0.1f) * Cooldown;
            SpawnProjectile(Vector3.zero);
        }
    }
}
