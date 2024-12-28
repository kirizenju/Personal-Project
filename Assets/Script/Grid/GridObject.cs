using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject 
{
   private GridSystem _system;
    private GridPosition _position;
    private List<Unit> unitList;
    public GridObject(GridSystem system, GridPosition position)
    {
        _system = system;
        _position = position;
        unitList = new List<Unit>();
    }
    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in unitList)
        {
            unitString += unit+"\n";
        }
        return _position.ToString()+"\n" + unitString;
    }
    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }
    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);  
    }
    public List<Unit> GetUnits()
    {   
        return unitList;
    }           
    public bool hasAnyUnit()
    {
        return unitList.Count > 0;
    }
}
