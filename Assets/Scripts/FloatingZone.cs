using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingZone : MonoBehaviour
{
    public bool IsStart;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.GetComponent<CharacterMovement>().MovementComponent.ToggleFloating(IsStart);
        }
    }
}
