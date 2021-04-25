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
        //float frac = Time.time - (int)Time.time;
        float scale = Curve.Evaluate(Time.time) * Scale;

        transform.localScale = new Vector3(scale, scale, 1);
    }
}
