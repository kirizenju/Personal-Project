using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance {get; private set;}
    public event EventHandler OnSelectedUnitChange;
    public event EventHandler OnSelectedActionChange;
    public event EventHandler OnActionStarts;
    public event EventHandler<bool> OnBusyChanged;
    
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private BaseAction selectedAction;
    private bool isBusy;
    private void Awake()
    {
        //singleton
        if(Instance != null)
        {
            Debug.LogError("There is more than 1 UnitActionSystem~" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }
    private void Update()
    {
        if (isBusy) return;
        if (!TurnSystem.Instance.IsPlayerTurn()) { return; }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (TryHandleSelection()) return;
        HandleSelectedAction();

    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition))
                {
                    return ;
                }
            if (!selectedUnit.TrySpendAPToAction(selectedAction))
                {
                    return ;
                }

                SetBusy();
                selectedAction.TakeAction(mouseGridPosition, ClearBusy);       
            OnActionStarts?.Invoke(this,EventArgs.Empty);
        }
    }
    private void SetBusy() { 
        isBusy = true; 
        OnBusyChanged?.Invoke(this,isBusy);
    }
    private void ClearBusy() {  isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }
    private bool TryHandleSelection()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                //TryGetComponet return true when raycastHit
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if(unit==selectedUnit) { return false; }
                    SetSelectedUnit(unit);
                    return true;
                }
                if (unit.IsEnemy())
                {
                    //Click on enemy
                    return false;  
                }
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        selectedAction = unit.GetMoveAction();
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
        OnSelectedActionChange?.Invoke(this, EventArgs.Empty);
    }
    public void SetSelectedAction(BaseAction action)
    {
        selectedAction = action;
        OnSelectedActionChange?.Invoke(this, EventArgs.Empty);
    }
    public Unit GetSelectUnit()
    {
        return selectedUnit;
    }
    public BaseAction GetSelectedAction() { return selectedAction; }
}
