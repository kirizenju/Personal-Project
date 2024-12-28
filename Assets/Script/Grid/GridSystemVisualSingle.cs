using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer m_Renderer;
    public void Show()
    {
        m_Renderer.enabled = true;
    }

    public void Hide()
    {
        m_Renderer.enabled = false;
    }
}
