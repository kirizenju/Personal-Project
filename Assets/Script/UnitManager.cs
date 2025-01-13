using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance {  get; private set; }  
    private List<Unit> unitList;
    private List<Unit> friendlyUnits;
    private List<Unit> enemyUnits;
    private void Awake()
    {
        //singleton
        if (Instance != null)
        {
            Debug.LogError("There is more than 1 UnitManager~" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        unitList = new List<Unit>();
        friendlyUnits = new List<Unit>();
        enemyUnits = new List<Unit>();
    }
    private void Start()
    {
        Unit.OnAnySpawn += Unit_OnAnySpawn;
        Unit.OnAnyDead += Unit_OnAnyDead;
    }
    private void OnDestroy()
    {
        Unit.OnAnySpawn -= Unit_OnAnySpawn;
        Unit.OnAnyDead -= Unit_OnAnyDead;
    }

    private void Unit_OnAnyDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        unitList.Remove(unit);
        if (unit.IsEnemy())
        {
            enemyUnits.Remove(unit);
        }
        else
        {
            friendlyUnits.Remove(unit);
        }
    }

    private void Unit_OnAnySpawn(object sender, EventArgs e)
    {
        Unit unit=sender as Unit;
        unitList.Add(unit);
        if (unit.IsEnemy())
        {
            enemyUnits.Add(unit);
        }
        else
        {
            friendlyUnits.Add(unit); 
        }

    }
    public List<Unit> GetUnitList()
    {
        return unitList;
    }
    public List<Unit> GetEnemyList()
    {
        return enemyUnits;
    }
    public List<Unit> GetFriendlyList()
    {
        return friendlyUnits;
    }
}
