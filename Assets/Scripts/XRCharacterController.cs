using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

[RequireComponent(typeof(CharacterController)), RequireComponent(typeof(XROrigin))]
public class XRCharacterController : MonoBehaviour
{
    CharacterController _characterController;
    XROrigin _origin;
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _origin = GetComponent<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        _characterController.height = _origin.CameraInOriginSpaceHeight;
        _characterController.radius = Mathf.Clamp(_origin.CameraInOriginSpaceHeight / 2, 0.1f, 0.5f);
        _characterController.center = new Vector3(0, _origin.CameraInOriginSpaceHeight / 2, 0);
    }
}
