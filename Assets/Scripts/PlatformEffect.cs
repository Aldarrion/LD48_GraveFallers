using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformType
{
    Thorny,
};

public class PlatformEffect : MonoBehaviour
{
    public PlatformType Type;

    private bool active_ = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!active_)
            return;

        if (collision.collider.CompareTag("Player"))
        {
            Apply(collision.gameObject.GetComponent<CharacterLogic>());
        }
    }

    private void Apply(CharacterLogic character)
    {
        switch (Type)
        {
            case PlatformType.Thorny:
            {
                active_ = false;
                character.TakeDamage(1);
                GetComponent<SpriteRenderer>().color = Color.white;
                break;
            }
            default:
                break;
        }
    }
}
