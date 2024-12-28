using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance=4;

    private Vector3 targetPosition;
    private float moveSpeed = 4f;
    private float stoppingDistance = .1f;
    private float rotateSpeed = 10f;
    
    protected override void Awake()
    {
        base.Awake();
        //not moving to position of unit
        targetPosition = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isActive) { 
            return; 
        }
        HandleMovement();

    }
    private void HandleMovement()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
           
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
            isActive = false;
            onActionComplete();
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * rotateSpeed);
    }
    //Get vector3
    public override void TakeAction(GridPosition gridPosition,Action onActionComplete)
    {
        this.targetPosition=LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
        this.onActionComplete = onActionComplete;
    }
   
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition > result = new List<GridPosition>();
        GridPosition unitGridPosition=unit.GetGridPosition();
        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition =unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {                 
                   continue;
                }
                if (unitGridPosition == testGridPosition)
                {
                   
                    //same gridPosition
                    continue;
                }
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // gridPosition already occupied with another unit                
                    continue;
                }
                result.Add(testGridPosition);
                
            }
        }
        return result;
    }

    public override string GetActionName()
    {
        return "Move";
    }

}
