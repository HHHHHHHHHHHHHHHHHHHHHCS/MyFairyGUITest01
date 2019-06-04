using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

public class MyBagWindow : Window
{
    protected override void OnInit()
    {
        this.contentPane = UIPackage.CreateObject("08_Bag", "BagWindow").asCom;
    }
}
