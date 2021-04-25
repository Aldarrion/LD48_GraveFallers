using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLogic : MonoBehaviour
{
    public ParticleSystem BloodParticleSystem;

    public float ScreenShakeVelocityMin = -0.3f;
    public float ScreenShakeVelocityMax = -0.6f;

    public float ShakeTime = 0.3f;
    public float ShakeIntensity = 0.3f;

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
        
        var cameraController = GameManager.Instance.Cameras[PlayerId].GetComponent<CameraController>();
        if (cameraController)
        {
            cameraController.Shake(0.15f, 0.15f);
        }

        //BloodParticleSystem.Stop();
        BloodParticleSystem.Play();
    }

    public void Heal(int count)
    {
        LifeCount += count;
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
        //Debug.Log($"Landed with velocity: {velocity.y}");
    }

    private void Start()
    {
        var charComp = GetComponent<CharacterMovement>();
        if (charComp)
            charComp.MovementComponent.OnLanded += OnLanded;
    }
}
