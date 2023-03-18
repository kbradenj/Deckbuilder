using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{

    public Camera cam;
    float zoom = 10f;

    void Start()
    {
        cam  = Camera.main;
    }
  

    void Update()
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * zoom, 6, 20);
    }
}