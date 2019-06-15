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


    private void Awake()
    {
        mainUI = GetComponent<UIPanel>().ui;
        gTextField = mainUI.GetChild("n4").asTextField;
        joystick = new MyJoystick(mainUI);

        joystick.OnMove.Add(OnMoveCallBack);
        joystick.OnEnd.Add(OnEndCallBack);
    }

    private void OnMoveCallBack(EventContext context)
    {
        float degree = (float) context.data;
        gTextField.text = degree.ToString();
    }

    private void OnEndCallBack()
    {
        gTextField.text = "";
    }
}