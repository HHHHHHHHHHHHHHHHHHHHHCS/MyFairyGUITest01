using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

public class ScriptLoadPackage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GRoot.inst.SetContentScaleFactor(1920, 1080);
        UIPackage.AddPackage("FGUI/Package1");
        GComponent component = UIPackage.CreateObject("Package1", "Bg3").asCom;
        GRoot.inst.AddChild(component);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
