using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Unit : MonoBehaviour
{
    //private Transform headTransform;
    private  const int actionPointMax = 2;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnySpawn;
    public static event EventHandler OnAnyDead;

    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private ShootAction shootAction;
    private BaseAction[] baseActionArray;
    private int actionPoint = actionPointMax;
    private HealthSystem healthSystem;
    [SerializeField] private bool isEnemy;
    public bool IsEnemy()
    {
        return isEnemy;
    }
    public void Awake()
    {
        //headTransform = transform.Find("Eyebrows");
                
        healthSystem = GetComponent<HealthSystem>();   
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        shootAction=GetComponent<ShootAction>();
        baseActionArray=GetComponents<BaseAction>();

        foreach (var action in baseActionArray)
        {
            Debug.Log($"Unit has action: {action.GetActionName()}");
        }
    }
    private void Start()
    {
        gridPosition=LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition,this);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnturnChanged;
        healthSystem.onDead += HealthSystem_OnDead;
        OnAnySpawn?.Invoke(this,EventArgs.Empty);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition (gridPosition,this);
        Destroy(gameObject);
        OnAnyDead?.Invoke(this, EventArgs.Empty);
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
    public ShootAction GetShootAction() { return shootAction; }
    public GridPosition GetGridPosition() { return gridPosition; }
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
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
    public void Damage(int dmg)
    {
        healthSystem.Damage(dmg);
    }
    //public Vector3 GetHeadPosition()
    //{
       
    //    return headTransform.position; 
    //}
}
