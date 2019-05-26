using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class FGUIBoss : MonoBehaviour
{
    private GComponent mainUI;
    private GComponent bossCom;
    private GGroup group;

    private void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;
        group = mainUI.GetChild("n2").asGroup;
        bossCom = UIPackage.CreateObject("02_Boss", "Boss").asCom;
        mainUI.GetChild("n0").onClick.Add(() => PlayBossButton(bossCom));

    }

    private void PlayBossButton(GComponent targetCom)
    {
        group.visible = false;
        GRoot.inst.AddChild(targetCom);
        Transition t = targetCom.GetTransition("t0");
        t.Play(() =>
        {
            group.visible=true;
            GRoot.inst.RemoveChild(targetCom);
        });

    }
}