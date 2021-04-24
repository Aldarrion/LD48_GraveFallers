using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLogic : MonoBehaviour
{
    public int LifeCount { get; private set; }

    public void TakeDamage(int count)
    {
        LifeCount -= 1;
        Debug.Log($"Damage taken, life count: {LifeCount}");
    }

    void Awake()
    {
        LifeCount = 3;
    }

    void Update()
    {
        
    }
}
