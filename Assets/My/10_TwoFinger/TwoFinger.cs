using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FairyGUI;
using UnityEngine;

public class TwoFinger : MonoBehaviour
{
    public RectTransform p1, p2, center;

    private float startScale;
    private Camera mainCamera;


    void Start()
    {
        MyPinchGesture pg = new MyPinchGesture(GRoot.inst);
        pg.onBegin.Add(OnBegin);
        pg.onAction.Add(OnMove);
        pg.onEnd.Add(OnEnd);
        mainCamera = Camera.main;
    }

    private void OnBegin(EventContext context)
    {
        var pg = context.sender as MyPinchGesture;

        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(new Vector2(pg.center.x, Screen.height - pg.center.y));
        startScale = mainCamera.fieldOfView;

        if (Physics.Raycast(ray, out hit))
        {
            mainCamera.transform.DOLookAt(hit.point, 0.3f);
            p1.position = new Vector2(pg.pt1.x, Screen.height - pg.pt1.y);
            p2.position = new Vector2(pg.pt2.x, Screen.height - pg.pt2.y);
            center.position = new Vector2(pg.center.x, Screen.height - pg.center.y);
        }
    }

    private void OnMove(EventContext context)
    {
        var pg = context.sender as MyPinchGesture;
        mainCamera.fieldOfView = startScale * (1f / pg.scale);
        p1.position = new Vector2(pg.pt1.x, Screen.height - pg.pt1.y);
        p2.position = new Vector2(pg.pt2.x, Screen.height - pg.pt2.y);
        center.position = new Vector2(pg.center.x, Screen.height - pg.center.y);
    }

    private void OnEnd(EventContext context)
    {
    }
}