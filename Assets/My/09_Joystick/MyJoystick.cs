using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  FairyGUI;
using  DG.Tweening;

//EventDispatcher 事件收发
public class MyJoystick : EventDispatcher
{
    public EventListener OnMove { get; private set; }
    public EventListener OnEnd { get; private set; }


    //mainUI里的对象
    private GButton joystickButton;
    private GImage thumb;
    private GGraph touchArea;
    private GImage center;

    //摇杆属性
    private float initX;
    private float initY;
    private float startStageX;
    private float startStageY;
    private float lastStageX;
    private float lastStageY;
    private int touchID;
    public  float radius { get; set; }
    private Tweener tweener;

    public MyJoystick(GComponent mainUI)
    {
        OnMove=new EventListener(this,"onMove");
        OnEnd=new EventListener(this, "onEnd");

        joystickButton = mainUI.GetChild("Joystick").asButton;
        joystickButton.changeStateOnClick = false;
        thumb = joystickButton.GetChild("thumb").asImage;
        touchArea = mainUI.GetChild("JoystickArea").asGraph;
        center = mainUI.GetChild("JoystickCenter").asImage;
    }
}
