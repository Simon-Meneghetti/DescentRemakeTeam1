using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tachimetro : MonoBehaviour
{
    private Transform Lancetta;
    private const float MaxSpeedAngle = -94;
    private const float MinSpeedAngle = 94;
    private float speedMax;
    public Rigidbody target;
    private float speed; private void Awake()
    {
        Lancetta = transform.Find("lancetta");
        speed = 0;
        speedMax = 100;
    }
    void Update()
    {
        speed = target.velocity.magnitude * 3.6f;
        if (speed > speedMax)
            speed = speedMax;
        Lancetta.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
    }
    private float GetSpeedRotation()
    {
        float totalAngleSize = MinSpeedAngle - MaxSpeedAngle;
        float speedNormalized = speed / speedMax;
        return MinSpeedAngle - speedNormalized * totalAngleSize;
    }
}
