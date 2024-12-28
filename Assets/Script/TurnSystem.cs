using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;

    private int turnNumber;
    private bool isPlayerTurn;


    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn=!isPlayerTurn;
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }
    private void Awake()
    {
        //singleton
        if (Instance != null)
        {
            Debug.LogError("There is more than one TurnSystem instance in the scene!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        turnNumber = 1;
        isPlayerTurn = true;
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public void EndTurn()
    {
        isPlayerTurn = !isPlayerTurn;
        if (isPlayerTurn)
        {
            turnNumber++;
        }
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }
}
