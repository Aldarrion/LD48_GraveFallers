using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    public Vector2 Direction;

    private void Update()
    {
        Vector2 movement = Direction * Speed * Time.deltaTime;
        transform.position += new Vector3(movement.x, movement.y);
    }

    private void Disintegrate()
    {
        // TODO splash and stuff
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;

        if (other.CompareTag("PlayerHitbox"))
        {
            other.GetComponent<CharacterHitbox>().CharacterLogic.TakeDamage(1);
        }

        Disintegrate();
    }
}