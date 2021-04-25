using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLogic : MonoBehaviour
{
    private int _lifeCount;
    public int LifeCount
    {
        get
        {
            return _lifeCount;
        }
        private set
        {
            _lifeCount = value;
            GameManager.Instance.SetLifeCount(PlayerId, _lifeCount);
        }
    }

    public int PlayerId { get; private set; }

    public void TakeDamage(int count)
    {
        LifeCount -= count;
        Debug.Log($"Damage taken, life count: {LifeCount}");
    }

    public void Heal(int count)
    {
        LifeCount += count;
        Debug.Log($"Healed, life count: {LifeCount}");
    }

    public void Init(int playerId, int lifeCount)
    {
        PlayerId = playerId;
        LifeCount = lifeCount;
    }
}
