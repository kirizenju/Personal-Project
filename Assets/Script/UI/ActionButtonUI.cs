using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button btn;
    [SerializeField] private GameObject selectedGameObj;

    private BaseAction baseAcion;
    public void SetBaseAction(BaseAction action)
    {
        this.baseAcion = action;
        textMeshPro.text = action.GetActionName().ToUpper();
        btn.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(action);
        });
    }
    public void UppdateSelectedVisual()
    {
        BaseAction action = UnitActionSystem.Instance.GetSelectedAction();  
        selectedGameObj.SetActive(action==baseAcion);
    }
}
