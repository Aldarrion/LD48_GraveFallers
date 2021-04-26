using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLogic : MonoBehaviour
{
    public int MaxLives;
    public ParticleSystem BloodParticleSystem;
    public GameObject Hitbox;
    public GameObject Trail;
    public GameObject CharacterSprite;

    public float ScreenShakeVelocityMin = -0.3f;
    public float ScreenShakeVelocityMax = -0.6f;

    public float ShakeTime = 0.3f;
    public float ShakeIntensity = 0.3f;

    public float RespawnSpeed;

    public GameObject HeartSpawner;

    private int _lifeCount;
    
    private bool _isRespawning;
    private Vector3 _respawnPos;
    private float _timeToRespawn;
    private Vector3 _respawnDir;

    public int LifeCount
    {
        get
        {
            return _lifeCount;
        }
        private set
        {
            _lifeCount = Mathf.Max(0, value);
            GameManager.Instance.SetLifeCount(PlayerId, _lifeCount);
        }
    }

    public int PlayerId { get; private set; }

    private void ToggleRespawning(bool isRespawning)
    {
        _isRespawning = isRespawning;
        Trail.SetActive(isRespawning);
        CharacterSprite.SetActive(!isRespawning);
        Hitbox.SetActive(!isRespawning);
        GetComponent<BoxCollider2D>().enabled = !isRespawning;
        GetComponent<CharacterMovement>().enabled = !isRespawning;
    }

    public void TakeDamage(int count)
    {
        LifeCount -= count;
        if (LifeCount <= 0)
        {
            // Start respawn
            _respawnPos = GameManager.Instance.LevelGenerator.GetRespawnPosition(transform.position);
            _respawnPos.z = transform.position.z;
            _timeToRespawn = Vector3.Distance(transform.position, _respawnPos) / RespawnSpeed;
            _respawnDir = (_respawnPos - transform.position).normalized;
            ToggleRespawning(true);
        }
        return;
        var cameraController = GameManager.Instance.Cameras[PlayerId].GetComponent<CameraController>();
        if (cameraController)
        {
            cameraController.Shake(0.15f, 0.15f);
        }

        BloodParticleSystem.Stop();
        BloodParticleSystem.Play();

        Instantiate(HeartSpawner, transform.position, Quaternion.identity);
    }

    public bool Heal(int count)
    {
        int newCount = Mathf.Min(MaxLives, LifeCount + count);
        if (newCount == LifeCount)
            return false;

        LifeCount = newCount;
        // TODO healing effects

        return true;
    }

    public void Init(int playerId, int lifeCount)
    {
        PlayerId = playerId;
        LifeCount = lifeCount;
    }

    private float Interpolate(float min, float max, float current, float minValue)
    {
        float c = Mathf.Clamp(current, max, min);
        float t = (c - min) / (max - min);
        float maxValue = max * minValue / min;
        return Mathf.Lerp(minValue, maxValue, t);
    }

    public void OnLanded(Vector2 velocity)
    {
        if (velocity.y < -0.3f)
        {
            var cameraController = GameManager.Instance.Cameras[PlayerId].GetComponent<CameraController>();
            if (cameraController)
            {
                cameraController.Shake(
                    Interpolate(ScreenShakeVelocityMin, ScreenShakeVelocityMax, velocity.y, ShakeTime),
                    Interpolate(ScreenShakeVelocityMin, ScreenShakeVelocityMax, velocity.y, ShakeIntensity)
                );
            }
        }
    }

    private void Start()
    {
        var charComp = GetComponent<CharacterMovement>();
        if (charComp)
            charComp.MovementComponent.OnLanded += OnLanded;

        var particleSystem = Trail.GetComponent<ParticleSystem>();
        particleSystem.startColor = GameManager.Instance.PlayerColors[PlayerId];
        Trail.GetComponent<SpriteRenderer>().color = GameManager.Instance.PlayerColors[PlayerId];
    }

    private void Update()
    {
        if (_isRespawning)
        {
            _timeToRespawn -= Time.deltaTime;
            if (_timeToRespawn <= 0)
            {
                transform.position = _respawnPos;
                ToggleRespawning(false);
                Heal(3);
            }
            else
            {
                transform.position += _respawnDir * RespawnSpeed * Time.deltaTime;
            }
        }
    }
}
