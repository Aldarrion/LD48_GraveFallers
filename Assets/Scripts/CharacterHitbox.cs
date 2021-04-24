using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHitbox : MonoBehaviour
{
    public CharacterLogic CharacterLogic { get; private set; }

    private void Start()
    {
        CharacterLogic = transform.parent.GetComponent<CharacterLogic>();
        Debug.Assert(CharacterLogic != null);
    }
}
