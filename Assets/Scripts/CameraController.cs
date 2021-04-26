using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform PlayerToFollow;
    public float VerticalOffset;

    public float ShakeVigor = 100;

    public void Shake(float time, float intensity)
    {
        _shakeTime += time;
        _shakeIntensity = intensity;
    }

    private float _shakeTime;
    private float _shakeIntensity;
    private Vector3 _cameraPos;

    private void Start()
    {
        _cameraPos = transform.position;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameRunning)
            return;

        Vector3 offset = Vector3.zero;
        if (_shakeTime > 0)
        {
            _shakeTime -= Time.deltaTime;
            float t = _shakeTime * ShakeVigor;
            offset.x = Mathf.PerlinNoise(t, t) - 0.5f;
            offset.y = Mathf.PerlinNoise(t + 100, t + 100) - 0.5f;
            offset *= _shakeIntensity;
        }

        _cameraPos = new Vector3(_cameraPos.x, PlayerToFollow.transform.position.y + VerticalOffset, _cameraPos.z);
        transform.position = offset + _cameraPos;
    }
}
