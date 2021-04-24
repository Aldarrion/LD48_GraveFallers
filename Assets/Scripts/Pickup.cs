using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    Heart
}

public class Pickup : MonoBehaviour
{
    public PickupType Type;

    private void Apply(CharacterLogic character)
    {
        switch (Type)
        {
            case PickupType.Heart:
                character.Heal(1);
                break;
            default:
                Debug.Assert(false);
                break;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Apply(other.GetComponent<CharacterLogic>());
        }
    }
}
