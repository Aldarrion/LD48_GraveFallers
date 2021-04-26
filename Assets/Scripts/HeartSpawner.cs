using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
    public float Distance;
    public float TimeToActivate;
    public ParticleSystem TrailParticles;
    public ParticleSystem ExplosionParticles;
    public GameObject HeartPrefab;

    private float _timeLeft;
    private float _speed;
    private bool _isActivated;

    private void Start()
    {
        _speed = Distance / TimeToActivate;
        _timeLeft = TimeToActivate;
    }

    private void Update()
    {
        if (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
            transform.position += Vector3.up * _speed * Time.deltaTime;
        }
        else if (!_isActivated)
        {
            _isActivated = true;
            TrailParticles.Stop();
            ExplosionParticles.Play();

            var item = Instantiate(HeartPrefab, transform.position, Quaternion.identity);
            GameManager.Instance.OnItemSpawned(item);
        }
        else if (!ExplosionParticles.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
