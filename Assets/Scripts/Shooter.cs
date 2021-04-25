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
    private float _currentCooldown;

    private void Start()
    {
        _currentCooldown = Cooldown;
        float rotation = (int)Direction * 90;
        transform.eulerAngles = new Vector3(0, 0, rotation);
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

    void Update()
    {
        _currentCooldown -= Time.deltaTime;
        if (_currentCooldown <= 0)
        {
            _currentCooldown = Cooldown + Random.Range(-0.1f, 0.1f) * Cooldown;
            GameObject projectileObject = Instantiate(Projectile, ProjectileSpawnPoint.position, Quaternion.identity);
            
            var projectile = projectileObject.GetComponent<Projectile>();
            projectile.Direction = GetDirection();
            projectile.Speed = Speed;
        }
    }
}
