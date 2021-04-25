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

    private bool Apply(CharacterLogic character)
    {
        switch (Type)
        {
            case PickupType.Heart:
                return character.Heal(1);
            default:
                Debug.Assert(false);
                break;
        }
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Apply(other.GetComponent<CharacterLogic>()))
            {
                Destroy(gameObject);
            }
        }
    }
}
