using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

public class MyBagWindow : Window
{
    private GList list;
    private GButton playerView;

    public MyBagWindow(GButton view)
    {
        playerView = view;
    }

    protected override void OnInit()
    {
        this.contentPane = UIPackage.CreateObject("08_Bag", "BagWindow").asCom;
        list = contentPane.GetChild("itemList").asList;
        list.itemRenderer = RenderListItem;


        list.numItems = 20;
        for (int i = 0; i < list.numItems - 10; i++)
        {
            GButton button = list.GetChildAt(i).asButton;
            button.onClick.Add(()=>ClickItem(button));
        }
    }

    private void RenderListItem(int index, GObject obj)
    {
        GButton button = obj.asButton;
        button.icon = UIPackage.GetItemURL("08_Bag", "i" + index);
        button.title = index.ToString();
    }

    private void ClickItem(GButton btn)
    {
        playerView.title = btn.title;
        playerView.icon = btn.icon;
    }
}