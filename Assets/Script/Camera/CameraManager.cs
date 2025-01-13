using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCamera;
    private void Start()
    {
        BaseAction.OnAnyActionStart += BaseAction_OnAnyActionStart;
        BaseAction.OnAnyActionComplete += BaseAction_OnAnyActionComplete;
        HideCamera();
    }

    private void BaseAction_OnAnyActionComplete(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideCamera();
                break;

        }
    }

    private void BaseAction_OnAnyActionStart(object sender, EventArgs e)
    {
        switch(sender)
        {
            case ShootAction shootAction:
                Unit shootUnit=shootAction.GetUnit();
                Unit targetUnit=shootAction.GetTargetUnit();
                Vector3 cameraHeight = Vector3.up * 1.7f;
                Vector3 shootDir=(targetUnit.GetWorldPosition()-shootUnit.GetWorldPosition()).normalized;
                float shoulderAmount = 0.5f;
                Vector3 shoulderOffset=Quaternion.Euler(0,90,0)*shootDir*shoulderAmount;
                Vector3 cameraPosition = shootUnit.GetWorldPosition() + cameraHeight + shoulderOffset+(shootDir*-1);
                actionCamera.transform.position = cameraPosition;
                actionCamera.transform.LookAt(targetUnit.GetWorldPosition()+cameraHeight);
                ShowCamera();
                break;

        }
    }

    private void ShowCamera()
    {
          actionCamera.SetActive(true);
    }
    private void HideCamera()
    {
          actionCamera.SetActive(false);
    }
}
