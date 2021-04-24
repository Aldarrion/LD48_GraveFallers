using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform PlayerToFollow;
    public float VerticalOffset;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, PlayerToFollow.transform.position.y + VerticalOffset, transform.position.z);
    }
}
