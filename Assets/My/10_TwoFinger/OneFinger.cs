using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

public class OneFinger : MonoBehaviour
{
    public Transform target;

    private void Awake()
    {

        SwipeGesture gesture1 = new SwipeGesture(GRoot.inst);
        gesture1.onMove.Add(OnSwipeMove);
        gesture1.onEnd.Add(OnSwipeEnd);
    }

    void OnSwipeMove(EventContext context)
    {
        SwipeGesture gesture = (SwipeGesture)context.sender;
        Vector3 v = new Vector3 {y = -gesture.delta.x};
        target.Rotate(v, Space.World);
    }

    void OnSwipeEnd(EventContext context)
    {
        SwipeGesture gesture = (SwipeGesture)context.sender;

    }
}
