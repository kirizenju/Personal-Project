using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }


    private int maxShootDistance = 4;
    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }
    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;
    private float rotateSpeed = 10f;
    private int dmg=40;

    private void Update()
    {
        if (!isActive) 
        {
            
            return;
        }
        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                HandleAiming();
                break;
            case State.Shooting:
                HandleShooting();
                break;
            case State.Cooloff:
                HandleCooloff();
                break;
        }

    }
    public override string GetActionName()
    {
        return "Shoot";
    }

    public  List<GridPosition> GetValidActionGridPositionList(GridPosition gridPosition)
    {
        List<GridPosition> result = new List<GridPosition>();
        
        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                if (x * x + z * z > maxShootDistance * maxShootDistance)
                {
                    continue;
                }


                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // gridPosition is empty, no unit                
                    continue;
                }
                Unit targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    //both unit same team
                    continue;
                }
                result.Add(testGridPosition);

            }
        }
        return result;
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition grid = unit.GetGridPosition();
        return GetValidActionGridPositionList(grid);
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        

        targetUnit = LevelGrid.Instance.GetUnitOnGridPosition(gridPosition);

        state = State.Aiming;
        stateTimer = 1f;

        canShootBullet = true;
        ActionStart(onActionComplete);
    }


    private void HandleAiming()
    {
        Vector3 aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(aimDirection), Time.deltaTime * rotateSpeed);
        // Khi hết thời gian nhắm, chuyển sang trạng thái bắn
        if (stateTimer <= 0f)
        {
            TransitionToState(State.Shooting, 0.5f);
        }
    }

    private void HandleShooting()
    {
        if (canShootBullet)
        {
            canShootBullet = false;
            if (targetUnit != null)
            {
                Shoot();
            }
        }

        if (stateTimer <= 0f)
        {
            TransitionToState(State.Cooloff, 1f); // 1 giây cooldown
        }
    }


    private void HandleCooloff()
    {
        ActionComplete();
    }
    private void TransitionToState(State newState, float duration)
    {
        state = newState;
        stateTimer = duration;
    }
    public override int GetAPCost()
    {
        return 1;
    }
    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit= targetUnit,
            shootingUnit=unit
        });
        targetUnit.Damage(dmg);
    }
    public Unit GetTargetUnit()
    {
        return targetUnit;
    }
    public override EnemyAIAction GetEnemyAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100,
        };
    }
    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }
}