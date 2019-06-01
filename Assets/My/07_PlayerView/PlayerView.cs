using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private GComponent mainUI;
    private PlayerWindow playerWindow;

    private void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;
        playerWindow = new PlayerWindow();
        mainUI.GetChild("n0").onClick.Add(() => { playerWindow.Show(); });
    }
}