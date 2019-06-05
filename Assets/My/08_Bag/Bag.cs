using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

public class Bag : MonoBehaviour
{
    private GComponent mainUI;
    private GButton playerView;
    private MyBagWindow bagWindow;

    private void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;
        playerView = mainUI.GetChild("playerView").asButton;
        playerView.onClick.Add(UseItem);
        bagWindow = new MyBagWindow(playerView);
        bagWindow.SetXY(200,300);
        mainUI.GetChild("bagButton").onClick.Add(bagWindow.Show);
    }

    private void UseItem()
    {
        playerView.icon = null;
        playerView.title = "空白";
    }
}
