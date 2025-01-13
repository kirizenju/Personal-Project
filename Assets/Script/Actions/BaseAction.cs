using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStart;
    public static event EventHandler OnAnyActionComplete;

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
    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
        OnAnyActionStart?.Invoke(this,EventArgs.Empty);
    }
    protected void ActionComplete()
    {
        Debug.Log("Action completed. isActive set to false.");
        isActive = false;
        onActionComplete?.Invoke();
        OnAnyActionComplete?.Invoke(this, EventArgs.Empty);
    }
    public Unit GetUnit() { return unit; }
    public EnemyAIAction GetBestEnemyAction()
    {
        List<EnemyAIAction> enemyAIActions = new List<EnemyAIAction>();
        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();
        foreach(GridPosition gridPosition in validActionGridPositionList)
        {
            EnemyAIAction enemyAiAction=GetEnemyAction(gridPosition);
            enemyAIActions.Add(enemyAiAction);
        }
        if (enemyAIActions.Count > 0)
        {
            enemyAIActions.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyAIActions[0];
        }
        else
        {
            return null;    
        }
    }

    public abstract EnemyAIAction GetEnemyAction(GridPosition gridPosition);
    
}
