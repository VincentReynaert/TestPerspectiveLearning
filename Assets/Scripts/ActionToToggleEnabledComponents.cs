using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR.Samples.ControllerSample;

[DisallowMultipleComponent]
public class ActionToToggleEnabledComponents : MonoBehaviour
{

    [Tooltip("Action Reference that represents the control")]
    [SerializeField] private InputActionReference _leftActionReference = null;
    [SerializeField] private InputActionReference _rightActionReference = null;
    [SerializeField] private List<MonoBehaviour> _componentList = new List<MonoBehaviour>();


    protected virtual void OnEnable()
    {
        if (!(_leftActionReference == null || _leftActionReference.action == null))
            _leftActionReference.action.performed += OnActionPerformed;
        if (!(_rightActionReference == null || _rightActionReference.action == null))
            _rightActionReference.action.performed += OnActionPerformed;

    }

    protected virtual void OnDisable()
    {
        if (!(_leftActionReference == null || _leftActionReference.action == null))
            _leftActionReference.action.performed -= OnActionPerformed;
        if (!(_rightActionReference == null || _rightActionReference.action == null))
            _rightActionReference.action.performed -= OnActionPerformed;

    }
    protected void OnActionPerformed(InputAction.CallbackContext ctx)
    {
        foreach (MonoBehaviour item in _componentList)
        {
            item.enabled = !item.enabled;
        }
    }

}
