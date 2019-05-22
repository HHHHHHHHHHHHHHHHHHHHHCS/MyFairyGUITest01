using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class FGUIBoss : MonoBehaviour
{
    private GComponent mainUI;
    private GComponent bossCom;

    private void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;
        bossCom = UIPackage.CreateObject("Package2", "Boss").asCom;
        mainUI.GetChild("n0").onClick.Add(() => PlayBossButton(bossCom));
    }

    private void PlayBossButton(GComponent targetCom)
    {
        mainUI.GetChild("n0").visible = false;
        GRoot.inst.AddChild(targetCom);
        Transition t = targetCom.GetTransition("t0");
        t.Play();
    }
}