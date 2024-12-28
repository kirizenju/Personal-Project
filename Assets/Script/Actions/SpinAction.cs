using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    
    private float totalSpinAmount;
    
    void Update()
    {
        if (!isActive)
        {
            return;
        }
        float spinAmmount = 360 * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAmmount, 0);
        
        totalSpinAmount += spinAmmount;
        if (totalSpinAmount >= 360f)
        {
            isActive = false;
            onActionComplete();
        }    
    }
   
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        isActive = true;
        totalSpinAmount = 0f;
    }

    public override string GetActionName()
    {
        return "Spin";
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return new List<GridPosition> { unitGridPosition };
    }
    public override int GetAPCost()
    {
        return 2;
    }
}  