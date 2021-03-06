﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;

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
    public float radius { get; set; }
    private GTweener tweener;

    public MyJoystick(GComponent mainUI)
    {
        OnMove = new EventListener(this, "onMove");
        OnEnd = new EventListener(this, "onEnd");

        joystickButton = mainUI.GetChild("Joystick").asButton;
        joystickButton.changeStateOnClick = false;
        thumb = joystickButton.GetChild("thumb").asImage;
        touchArea = mainUI.GetChild("JoystickArea").asGraph;
        center = mainUI.GetChild("JoystickCenter").asImage;

        initX = center.x + center.width / 2;
        initY = center.y + center.height / 2;
        touchID = -1;
        radius = 150;

        touchArea.onTouchBegin.Add(OnTouchBegin);
        touchArea.onTouchMove.Add(OnTouchMove);
        touchArea.onTouchEnd.Add(OnTouchEnd);
    }

    public void OnTouchBegin(EventContext context)
    {
        if (touchID == -1)
        {
            var input = context.inputEvent;
            touchID = input.touchId;

            if (tweener != null)
            {
                tweener.Kill();
                tweener = null;
            }

            Vector2 localPos = new Vector2(input.x, input.y);
            localPos = GRoot.inst.GlobalToLocal(localPos);
            float posX = localPos.x;
            float posY = localPos.y;
            joystickButton.selected = true;

            lastStageX = posX;
            lastStageY = posY;
            startStageX = posX;
            startStageY = posY;

            center.visible = true;
            center.SetXY(posX - center.width / 2, posY - center.height / 2);
            joystickButton.SetXY(posX - joystickButton.width / 2, posY - joystickButton.height / 2);

            float deltaX = posX - initX;
            float deltaY = posY - initY;
            float degrees = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;
            thumb.rotation = degrees;
            context.CaptureTouch(); //捕获触摸 继续往下传递
        }
    }


    public void OnTouchMove(EventContext context)
    {
        InputEvent inputEvent = context.inputEvent;
        if (touchID != -1 && inputEvent.touchId == touchID)
        {
            Vector2 localPos = GRoot.inst.GlobalToLocal(new Vector2(inputEvent.x, inputEvent.y));
            float posX = localPos.x;
            float posY = localPos.y;
            float moveX = posX - lastStageX;
            float moveY = posY - lastStageY;

            lastStageX = posX;
            lastStageY = posY;
            float buttonX = joystickButton.x + moveX;
            float buttonY = joystickButton.y + moveY;

            float deltaX = buttonX + joystickButton.width / 2 - startStageX;
            float deltaY = buttonY + joystickButton.height / 2 - startStageY;

            float rad = Mathf.Atan2(deltaY, deltaX);
            float degree = rad * Mathf.Rad2Deg;
            thumb.rotation = degree + 90;

            //设置范围
            float maxX = radius * Mathf.Cos(rad);
            float maxY = radius * Mathf.Sin(rad);
            if (Mathf.Abs(deltaX) > Mathf.Abs(maxX))
            {
                deltaX = maxX;
            }

            if (Mathf.Abs(deltaY) > Mathf.Abs(maxY))
            {
                deltaY = maxY;
            }

            buttonX = startStageX + deltaX;
            buttonY = startStageY + deltaY;

            joystickButton.SetXY(buttonX - joystickButton.width / 2, buttonY - joystickButton.height / 2);

            OnMove.Call(degree); //执行回调方法
        }
    }

    public void OnTouchEnd(EventContext context)
    {
        InputEvent inputEvent = context.inputEvent;
        if (touchID != -1 && inputEvent.touchId == touchID)
        {
            touchID = -1;
            thumb.rotation = thumb.rotation + 270;
            center.visible = false;

            tweener = joystickButton
                .TweenMove(new Vector2(initX - joystickButton.width / 2, initY - joystickButton.height / 2), 0.3f)
                .OnComplete(
                    _ =>
                    {
                        tweener = null;
                        joystickButton.selected = false;
                        thumb.rotation = 0;
                        center.visible = true;
                        center.SetXY(initX - center.width / 2, initY - center.height / 2);
                    });

            OnEnd.Call(); //执行回调方法
        }
    }
}