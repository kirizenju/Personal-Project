using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Unit : MonoBehaviour
{
    private  const int actionPointMax = 2;

    public static event EventHandler OnAnyActionPointsChanged;

    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private int actionPoint = actionPointMax;

    [SerializeField] private bool isEnemy;
    public bool IsEnemy()
    {
        return isEnemy;
    }
    public void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray=GetComponents<BaseAction>();
    }
    private void Start()
    {
        gridPosition=LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition,this);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnturnChanged;
    }

    private void TurnSystem_OnturnChanged(object sender, EventArgs e)
    {
        if((IsEnemy()&&!TurnSystem.Instance.IsPlayerTurn()) ||
            (!IsEnemy()&& TurnSystem.Instance.IsPlayerTurn())) {

            actionPoint = actionPointMax;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
       
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            //Unit changed Grid position
            LevelGrid.Instance.UnitMoveGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;

        }

    }
    public MoveAction GetMoveAction(){ return moveAction; } 
    public SpinAction GetSpinAction(){ return spinAction; } 
    public GridPosition GetGridPosition() { return gridPosition; }
    public BaseAction[] GetBaseActionArray() {  return baseActionArray; }
    public bool TrySpendAPToAction(BaseAction action)
    {
        if (CanSpendAPToAction(action))
        {
            SpendAPToAction(action.GetAPCost());

            return true;
        }
        else { return false; }
    }
    public bool CanSpendAPToAction(BaseAction action) 
    { 
        if(actionPoint>= action.GetAPCost())
        {
            return true;
        }
        else { return false; }
    }
    private void SpendAPToAction(int amount)
    {
        actionPoint-=amount;
        OnAnyActionPointsChanged?.Invoke(this,EventArgs.Empty);
    }
    public int GetAPs()
    {
        return actionPoint;
    }
}
