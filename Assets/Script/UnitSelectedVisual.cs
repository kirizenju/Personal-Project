using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;
    private MeshRenderer meshRenderer;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

    }
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
    }
    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs empty) 
    {
       UpdateVisual();
    }
    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectUnit() == unit)
        {
            meshRenderer.enabled = true;

        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}
