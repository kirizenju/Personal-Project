using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnBtn;
    [SerializeField] private TextMeshProUGUI turnNumberTxt;
    [SerializeField] private GameObject enemyTurnVisualGameObeject;



    public void Start()
    {
        endTurnBtn.onClick.AddListener(()=>{
            TurnSystem.Instance.NextTurn();
        });
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnturnChanged;
        UpdateNextTurn();
        UpdateEnemyTurnVisual();

        
        UpdateEndTurnBtnVisual();
    }
    private void UpdateNextTurn()
    {
        turnNumberTxt.text="Turn " + TurnSystem.Instance.GetTurnNumber();
    }
    private void TurnSystem_OnturnChanged(object sender,EventArgs e)
    {
        UpdateNextTurn();
        UpdateEnemyTurnVisual();
        UpdateEndTurnBtnVisual();
    }
    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVisualGameObeject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
            
    }
    private void UpdateEndTurnBtnVisual()
    {
        endTurnBtn.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());

    }
}
