using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed;

    new Transform transform;

    void Start()
    {
        transform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
