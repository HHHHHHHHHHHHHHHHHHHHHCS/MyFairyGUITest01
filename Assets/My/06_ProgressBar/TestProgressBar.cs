using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class TestProgressBar : MonoBehaviour
{
    private GComponent mainUI;
    private GProgressBar progressBar;
    private GComboBox comboBox;

    private void Start()
    {
        mainUI = GetComponent<UIPanel>().ui;
        progressBar = mainUI.GetChild("n3").asProgress;
        progressBar.TweenValue(100, 5);
        comboBox = mainUI.GetChild("n2").asComboBox;
        comboBox.onChanged.Add(SetCompleteTime);

    }

    private void SetCompleteTime()
    {
        progressBar.value = 0;
        progressBar.TweenValue(100, Convert.ToInt32(comboBox.value));
    }
}
