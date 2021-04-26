using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public SpriteRenderer Pulser;
    public float PulseTime;

    [Space]
    public float Speed;
    public Vector2 Direction;

    private float _timeToLive = 10.0f;
    private float _currentPulseTime = 0;
    private float _pulseSign = 1;

    private void Start()
    {
        _currentPulseTime = Random.Range(0, PulseTime);
    }

    private void Update()
    {
        _timeToLive -= Time.deltaTime;
        if (_timeToLive <= 0 || transform.position.x < -(GameConsts.ROW_SIZE + 1) || transform.position.x > (GameConsts.ROW_SIZE + 1))
        {
            Disintegrate();
            return;
        }

        Vector2 movement = Direction * Speed * Time.deltaTime;
        transform.position += new Vector3(movement.x, movement.y);

        _currentPulseTime += _pulseSign * Time.deltaTime;
        if (_currentPulseTime >= PulseTime)
        {
            _pulseSign = -1;
            _currentPulseTime = PulseTime;
        }
        else if (_currentPulseTime <= 0)
        {
            _pulseSign = 1;
            _currentPulseTime = 0;
        }

        float a = _currentPulseTime / PulseTime;
        Pulser.color = new Color(Pulser.color.r, Pulser.color.g, Pulser.color.b, a);
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
            var logic = other.GetComponent<CharacterHitbox>().CharacterLogic;
            if (logic.IsInvlunerable)
                return;

            logic.TakeDamage(1);
        }

        Disintegrate();
    }
}
