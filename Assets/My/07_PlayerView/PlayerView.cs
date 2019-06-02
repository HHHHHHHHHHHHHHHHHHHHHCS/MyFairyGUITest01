using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public GameObject player;
    public RenderTexture playerRT;
    public Material imageMat;

    private GComponent mainUI;
    private PlayerWindow playerWindow;

    private void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;
        playerWindow = new PlayerWindow(player, playerRT, imageMat);
        mainUI.GetChild("n0").onClick.Add(() => { playerWindow.Show(); });
    }
}