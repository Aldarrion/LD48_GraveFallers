using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    public float Speed = 3;

    private MovementComponent _movementComponent;

    void Awake()
    {
        _movementComponent = new MovementComponent(transform, GetComponent<Collider2D>(), Speed, "");
    }

    void Update()
    {
        _movementComponent.Update();

        transform.position = new Vector3(Mathf.Max(Mathf.Min(transform.position.x, 6), -6), transform.position.y, transform.position.z);
    }
}
