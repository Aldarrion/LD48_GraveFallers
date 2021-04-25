using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsing : MonoBehaviour
{
    public AnimationCurve Curve;
    public float Speed = 1;
    public float Scale = 1;

    void Update()
    {
        float scale = Curve.Evaluate(Time.time * Speed) * Scale;

        transform.localScale = new Vector3(scale, scale, 1);
    }
}
