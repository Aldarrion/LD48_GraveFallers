using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    public float Speed = 3;


    private MovementComponent _movementComponent;

    void Start()
    {
        _movementComponent = new MovementComponent(transform, GetComponent<Collider2D>(), GetGroundHeight, Speed /*FindObjectOfType<ColliderManager>()*/);
    }

    void Update()
    {
        _movementComponent.Update();
    }

    public float GetGroundHeight()
    {
        return 10;
    }
}
