using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicator : MonoBehaviour
{
    public GameObject CameraToUse;
    public int PlayerToFollow;
    public bool IsActive;

    public GameObject LocatorCircle;

    private Camera _camera;
    private Transform _player;

    private void Start()
    {
        _camera = CameraToUse.GetComponent<Camera>();
        _player = GameManager.Instance.Players[PlayerToFollow].transform;
    }

    private void Update()
    {
        // Detect if player to follow is out of bounds
        // Move transform to matching X coodinate
        // Set image rotation to 0 if y < bottom bound or 180 if y > top bound
        // Set scale of the whole thing based on distace
        // Move to Y position based on bounding box of the circle - at the bottom account for the UI somehow
    }
}
