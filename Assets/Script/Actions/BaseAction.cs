using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();

    }
    public abstract string GetActionName();
    public abstract void TakeAction(GridPosition gridPosition, Action onActioneComplete);
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition) {
        HashSet<GridPosition> gridPositions = new HashSet<GridPosition>(GetValidActionGridPositionList());
        bool isValid = gridPositions.Contains(gridPosition);
        //Debug.Log($"IsValidActionGridPosition for {gridPosition}: {isValid}");
        return isValid;
    }
    public abstract List<GridPosition> GetValidActionGridPositionList(); 
    public virtual int GetAPCost()
    {
        return 1;
    }
}
