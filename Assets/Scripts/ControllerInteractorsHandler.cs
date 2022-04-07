using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerInteractorsHandler : MonoBehaviour
{
    public TeleportationProvider provider;
    public XRRayInteractor tpRay;
    public InputActionReference activate;
    public InputActionReference cancel;
    public InputActionReference select;

    public Transform reticle;
    private bool _isTpActive;
    private Coroutine updateReticle = null;

    // Start is called before the first frame update
    void Start()
    {
        activate.action.Enable();
        cancel.action.Enable();
        select.action.Enable();

        activate.action.performed += OnActivate;
        activate.action.canceled += OnActivateEnd;
        cancel.action.performed += OnCancel;

        SetTpRayActive(false);
    }

    void SetTpRayActive(bool active)
    {
        _isTpActive = active;
        reticle.gameObject.SetActive(active);
        tpRay.gameObject.SetActive(active);
        if (updateReticle != null) StopCoroutine(updateReticle);
        if (gameObject.activeInHierarchy && active) StartCoroutine(UpdateReticle());
    }
    IEnumerator UpdateReticle() {
        while (_isTpActive) 
        {
            if (tpRay.TryGetCurrent3DRaycastHit(out RaycastHit hit)) 
            {
                Vector2 selVec = select.action.ReadValue<Vector2>();
                reticle.rotation = Quaternion.Euler(0,transform.eulerAngles.y,0) * Quaternion.LookRotation(new Vector3(selVec.x, 0, selVec.y));
                reticle.position = hit.point;
            }
            yield return null;
        }
    }

    private void OnActivate(InputAction.CallbackContext obj)
    {
        SetTpRayActive(true);
    }

    private void OnActivateEnd(InputAction.CallbackContext obj)
    {
        if (_isTpActive) 
        {
            if (tpRay.TryGetCurrent3DRaycastHit(out RaycastHit hit)) 
            {
                var tpReq = new TeleportRequest() {
                    destinationPosition = hit.point,
                    destinationRotation = reticle.rotation,
                    matchOrientation = MatchOrientation.TargetUpAndForward
                };
                provider.QueueTeleportRequest(tpReq);
            }
        }
        SetTpRayActive(false);
    }

    private void OnCancel(InputAction.CallbackContext obj)
    {
        SetTpRayActive(false);
    }
}
