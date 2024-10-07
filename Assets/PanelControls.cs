using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControls : MonoBehaviour
{
    private GameObject instructionCanvas;

    // Start is called before the first frame update
    void Start()
    {
        instructionCanvas = GameObject.FindGameObjectWithTag("PlanetOverviewPanel");
    }

    public void ClosePanel()
    {
        instructionCanvas.SetActive(false);
    }
}
