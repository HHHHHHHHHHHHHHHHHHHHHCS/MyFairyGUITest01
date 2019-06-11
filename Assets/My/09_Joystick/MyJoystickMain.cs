using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

public class MyJoystickMain : MonoBehaviour
{
    private GComponent mainUI;
    private GTextField gTextField;
    private MyJoystick joystick;


    private void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;
        gTextField = mainUI.GetChild("n4").asTextField;
        joystick = new MyJoystick(mainUI);
    }
}
