using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.CustomPlugins;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using FairyGUI;
using UnityEngine;
using UnityEngine.UI;

public class TwoFinger_Head : MonoBehaviour
{
    public Text text1, text2;
    public RectTransform p1, p2, center;

    public Transform farPos, nearPos, headPos;

    //3个阶段 3->2 放大  2->1 只能局部放大 1->0 不能选择其他部位
    private int pn = 3;

    private Vector3 lastPos;
    private Quaternion lastQua;
    private float lastP32Scale = 1, lastP21Scale = 1;
    private float p32Scale = 1, p21Scale = 1;
    private Camera mainCamera;

    public Dictionary<string, Action> aa = new Dictionary<string, Action>();

    void Start()
    {
        MyPinchGesture pg = new MyPinchGesture(GRoot.inst);
        pg.onBegin.Add(OnBegin);
        pg.onAction.Add(OnMove);
        pg.onEnd.Add(OnEnd);
        mainCamera = Camera.main;

        /*
        DOTween.To(() => nearPos.position, x =>
            mainCamera.transform.position = x, headPos.position
            , 1f);
        DOTween.To(PureQuaternionPlugin.Plug(), () => nearPos.rotation,
            x => mainCamera.transform.rotation = x,
            headPos.rotation,
            1f);
       */
    }

    private void Update()
    {
        text1.text = $"{pn}___{p32Scale}___{p21Scale}";
    }

    private void OnBegin(EventContext context)
    {
        var pg = context.sender as MyPinchGesture;



        if (pn == 2 && p32Scale <= 0.25f)
        {
            pn = 1;
            p21Scale = 1;
            lastPos = mainCamera.transform.position;
            lastQua = mainCamera.transform.rotation;
        }

        lastP32Scale = p32Scale;
        lastP21Scale = p21Scale;

    }

    private void OnMove(EventContext context)
    {
        var pg = context.sender as MyPinchGesture;

        float t = 0f;
        const float constVal = 1;
        Transform startTarget = null, endTarget = null;
        bool useSelf = false;

        if (pn == 3 || pn == 2)
        {
            p32Scale = Mathf.Clamp(lastP32Scale * (1f / pg.scale), 0.01f, 1f);
            pn = p32Scale == 1 ? 3 : 2;
            t = p32Scale / constVal;
            startTarget = nearPos;
            endTarget = farPos;
        }
        else if (pn == 1)
        {
            p21Scale = Mathf.Clamp(lastP21Scale * (1f / pg.scale), 0.01f, 5f);
            if (p21Scale > constVal)
            {
                t = p21Scale / 5;
                startTarget = nearPos;
                endTarget = farPos;
            }
            else
            {
                t = p21Scale / constVal;
                startTarget = headPos;
                useSelf = true;
            }
        }

        Vector3 endPos;
        Quaternion endRot;
        text2.text = t.ToString();
        if (useSelf)
        {
            endPos = Vector3.Lerp(startTarget.position, lastPos, t);
            endRot = Quaternion.Lerp(startTarget.rotation, lastQua, t);
        }
        else
        {
            endPos = Vector3.Lerp(startTarget.position, endTarget.position, t);
            endRot = Quaternion.Lerp(startTarget.rotation, endTarget.rotation, t);
        }

        mainCamera.transform.DOMove(endPos, 0.5f);
        mainCamera.transform.DORotateQuaternion(endRot, 0.5f);


        p1.position = new Vector2(pg.pt1.x, Screen.height - pg.pt1.y);
        p2.position = new Vector2(pg.pt2.x, Screen.height - pg.pt2.y);
        center.position = new Vector2(pg.center.x, Screen.height - pg.center.y);
    }

    private void OnEnd(EventContext context)
    {
        if (pn == 1)
        {
            if (p21Scale >= 1.25f)
            {
                pn = 2;
                p32Scale = (p21Scale - 1f) / 4f;
            }
        }
    }
}

/*
 
    private void OnMove(EventContext context)
    {
        var pg = context.sender as MyPinchGesture;
        nowScale = Mathf.Clamp01(nowScale * (1f / pg.scale));

        float t = 0f;
        const float constVal = 0.5f;
        Transform startTarget = null, endTarget = null;

        if (nowScale >= constVal)
        {
            pn = nowScale == 1 ? 3 : 2;
            t = (nowScale - constVal) / (1 - constVal);
            startTarget = nearPos;
            endTarget = farPos;
        }
        else
        {
            pn = 1;
            t = nowScale / constVal;
            startTarget = targetPos;
            endTarget = nearPos;
        }

        var endPos = Vector3.Lerp(startTarget.position, endTarget.position, t);
        mainCamera.transform.DOMove(endPos, 0.25f);
        var endRot = Quaternion.Lerp(startTarget.rotation, endTarget.rotation, t);
        mainCamera.transform.DORotateQuaternion(endRot, 0.25f);

        p1.position = new Vector2(pg.pt1.x, Screen.height - pg.pt1.y);
        p2.position = new Vector2(pg.pt2.x, Screen.height - pg.pt2.y);
        center.position = new Vector2(pg.center.x, Screen.height - pg.center.y);
    }

    */