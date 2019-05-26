using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;
using DG.Tweening;


public class AddValue : MonoBehaviour
{
    private GComponent mainUI;
    private GComponent addValueCom;
    private int startValue = 10000;
    private int endValue;

    private void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;
        addValueCom = UIPackage.CreateObject("03_AddValue", "AddValue").asCom;
        addValueCom.GetTransition("t0").SetHook("AddValue", AddAttackValue);
        mainUI.GetChild("n0").onClick.Add(PlayAddValue);
    }

    private void PlayAddValue()
    {
        mainUI.GetChild("n0").visible = false;
        GRoot.inst.AddChild(addValueCom);
        Transition t0 = addValueCom.GetTransition("t0");
        int addValue = Random.Range(1000, 3000);
        endValue = startValue + addValue;
        addValueCom.GetChild("n2").text = startValue.ToString();
        addValueCom.GetChild("n5").text = addValue.ToString();
        t0.Play(() =>
        {
            mainUI.GetChild("n0").visible = true;
            GRoot.inst.RemoveChild(addValueCom);
            startValue = endValue;

        });
    }

    private void AddAttackValue()
    {
        DOTween.To(() => ((float) startValue)
                , x => { addValueCom.GetChild("n2").text = Mathf.Floor(x).ToString(); }
                , endValue, 1)
            .SetEase(Ease.Linear)
            .SetUpdate(true);//不受时间缩放的影响
    }
}