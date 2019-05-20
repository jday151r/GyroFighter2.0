using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform focus;
    public Transform anchor;
    public Vector3 offset;
    public float lerp;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, ((focus.position + anchor.position) / 2) + offset, lerp);
        anchor.position = new Vector3(0, 0, transform.position.z);
    }
}