using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class XRCameraFOV : MonoBehaviour
{
    [SerializeField, Range(0.00001f, 180)] float FOV = 120;
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        camera.fieldOfView = FOV;
    }
    void Update()
    {
        camera.fieldOfView = FOV;
    }
    void FixedUpdate()
    {
        camera.fieldOfView = FOV;
    }
}
