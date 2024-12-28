using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro _textMeshPro;
   private GridObject gridObject;
    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }
    private void Update()
    {
        if (gridObject != null)
        {
            _textMeshPro.text = gridObject.ToString();
           
        }
    }
}
