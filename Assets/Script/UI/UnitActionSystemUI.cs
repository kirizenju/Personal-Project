using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI apText;

    private List<ActionButtonUI> actionButtonUIList;


    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        UnitActionSystem.Instance.OnActionStarts += UnitActionSystem_OnActionStarts;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnturnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        UpdateAP();
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateAP();
    }

    private void TurnSystem_OnturnChanged(object sender, EventArgs e)
    {
        UpdateAP();
    }

    private void UnitActionSystem_OnActionStarts(object sender, EventArgs e)
    {
        UpdateAP();
    }

    private void UnitActionSystem_OnSelectedUnitChange(object sender, System.EventArgs e)
    {
        UpdateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateAP();
    }
    private void UnitActionSystem_OnSelectedActionChange(object sender, System.EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform t in actionButtonContainerTransform)
        {
            Destroy(t.gameObject);
        }
        actionButtonUIList.Clear();
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectUnit();
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
           Transform actionBtnTransform= Instantiate(actionButtonPrefab, actionButtonContainerTransform);
           ActionButtonUI actionButtonUI=actionBtnTransform.GetComponent<ActionButtonUI>();
           actionButtonUI.SetBaseAction(baseAction);
            actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void ClearUnitActionButtons()
    {
        foreach (Transform button in actionButtonContainerTransform)
        {
            Destroy(button.gameObject);
        }
    }

    private void UpdateUnitActionButtons()
    {
        ClearUnitActionButtons();
        CreateUnitActionButtons();
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange -= UnitActionSystem_OnSelectedUnitChange;
    }
    private void UpdateSelectedVisual()
    {
        foreach(ActionButtonUI actionButonUI in actionButtonUIList)
        {
            actionButonUI.UppdateSelectedVisual();
        }
    }
    private void UpdateAP()
    {
        Unit selectedUnit=UnitActionSystem.Instance.GetSelectUnit();
        apText.text = "AP: " + selectedUnit.GetAPs();
    }
}
