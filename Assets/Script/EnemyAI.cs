using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float timer;
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }
    private State state;
    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;          
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;

            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (TryTakeEnemyActionAI(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        // No enemy can take action
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;

            case State.Busy:
                break;
        }
    }
    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state= State.TakingTurn;
    }
    private bool TryTakeEnemyActionAI(Action enemyAIAction)
    {
        foreach(Unit enemyUnit in UnitManager.Instance.GetEnemyList())
        {
            if(TryTakeEnemyActionAI(enemyUnit, enemyAIAction))
            {
                return true;
            }
           
        }
        return false;
    }

    private bool TryTakeEnemyActionAI(Unit enemyUnit, Action onEnemyAIAction)
    {
        EnemyAIAction bestAction= null;
        BaseAction bestBaseAction = null;
        foreach(BaseAction baseAction in enemyUnit.GetBaseActionArray()){
            if (!enemyUnit.CanSpendAPToAction(baseAction))
            {
                continue;
            }
            if(bestAction == null)
            {
                bestAction=baseAction.GetBestEnemyAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAI=baseAction.GetBestEnemyAction();
                if(testEnemyAI!=null&&testEnemyAI.actionValue>bestAction.actionValue)
                {
                    bestAction = testEnemyAI;
                    bestBaseAction=baseAction;
                }
            }
            baseAction.GetBestEnemyAction();
        }

        if(bestAction != null&&enemyUnit.TrySpendAPToAction(bestBaseAction)) {
            bestBaseAction.TakeAction(bestAction.gridPosition, onEnemyAIAction);
            return true;
        }
        else
        {
            return false;
        }
       
    }

    private void TurnSystem_OnTurnChanged(object sender,EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn()) {
            state=State.TakingTurn;
            timer = 2f;
        }

       
    }
}
