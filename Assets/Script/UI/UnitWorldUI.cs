using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UnitWorldUI : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI actionPointText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyApChanged;
        healthSystem.onDmg += HealthSystem_OnDmg;
        UpdateAPText();
        UpdateHealthBar();
    }

    private void HealthSystem_OnDmg(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void Unit_OnAnyApChanged(object sender, EventArgs e)
    {
        UpdateAPText();
    }

    private void UpdateAPText()
    {
        actionPointText.text=unit.GetAPs().ToString();
    }
    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount=healthSystem.GetHealthNormalized();
    }
}
