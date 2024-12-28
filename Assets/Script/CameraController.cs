using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera CVM;

    private const float MinFollowYOffset = 2f;
    private const float MaxFollowYOffset = 12f;
    public float zoomSpeed = 5f;
    private float targetFollowYOffset;
    private float zoomAmount = 1f;
    private CinemachineTransposer cinemachineTransposer;
    void Start()
    {
        // Cache the CinemachineTransposer component at the start
        cinemachineTransposer = CVM.GetCinemachineComponent<CinemachineTransposer>();
        // Initialize targetFollowYOffset with the current y offset
        targetFollowYOffset = cinemachineTransposer.m_FollowOffset.y;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
        
    }

    private void HandleZoom()
    {
        // Check for mouse scroll input
        if (Input.mouseScrollDelta.y != 0)
        {
            // Adjust the target follow offset based on scroll direction
            targetFollowYOffset -= Input.mouseScrollDelta.y * zoomAmount;
            // Clamp the target follow offset to the specified range
            targetFollowYOffset = Mathf.Clamp(targetFollowYOffset, MinFollowYOffset, MaxFollowYOffset);
        }
        Vector3 followOffset = cinemachineTransposer.m_FollowOffset;
        followOffset.y = Mathf.Lerp(followOffset.y, targetFollowYOffset, Time.deltaTime * zoomSpeed);
        cinemachineTransposer.m_FollowOffset = followOffset;
        //if(Input.mouseScrollDelta.y > 0) { targetFollowYOffset.y -= zoomAmount; }
        //if (Input.mouseScrollDelta.y < 0) { targetFollowYOffset.y += zoomAmount; }
        //followOffset.y=Mathf.Clamp(followOffset.y,MinFollowYOffset,MaxFollowYOffset);
        //followOffset.y = Mathf.Lerp(followOffset.y, targetFollowYOffset, Time.deltaTime * zoomSpeed);
        //cinemachineTransposer.m_FollowOffset = followOffset;
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.Q)) { rotationVector.y = +1f; }
        if (Input.GetKey(KeyCode.E)) { rotationVector.y = -1f; }
        float rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    private void HandleMovement()
    {
        Vector3 inputMoveDirection = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) { inputMoveDirection.z = +1f; }
        if (Input.GetKey(KeyCode.S)) { inputMoveDirection.z = -1f; }
        if (Input.GetKey(KeyCode.A)) { inputMoveDirection.x = -1f; }
        if (Input.GetKey(KeyCode.D)) { inputMoveDirection.x = +1f; }
        float moveSpeed = 10f;
        Vector3 moveVector = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;
        transform.position += inputMoveDirection * moveSpeed * Time.deltaTime;

       

    }
}
