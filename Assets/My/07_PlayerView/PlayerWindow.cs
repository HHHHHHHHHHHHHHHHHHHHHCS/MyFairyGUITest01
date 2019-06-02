using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

public class PlayerWindow : Window
{
    /// <summary>
    /// 这个Y是反着算的  信了邪了
    /// </summary>
    private Vector3 pos = new Vector3(40, 150, 0);

    private Vector2 scale = new Vector3(2, 2, 0);

    private GameObject player;
    private RenderTexture playerRT;
    private Material imageMat;

    public PlayerWindow(GameObject player, RenderTexture playerRT, Material imageMat)
    {
        this.player = player;
        this.playerRT = playerRT;
        this.imageMat = imageMat;
    }

    protected override void OnInit()
    {
        contentPane = UIPackage.CreateObject("07_PlayerView", "PlayerWindow").asCom;
        var holder = contentPane.GetChild("n4").asGraph;

        var image = new Image {texture = new NTexture(playerRT), material = imageMat};

        holder.SetNativeObject(image);

        image.position = pos;
        image.scale = scale;

        contentPane.GetChild("leftButton").onClick.Add(() => RotatePlayer(true));
        contentPane.GetChild("rightButton").onClick.Add(() => RotatePlayer(false));
    }

    private void RotatePlayer(bool isLeft)
    {
        player.transform.Rotate((isLeft ? 1 : -1) * 30 * Vector3.up, Space.World);
    }
}