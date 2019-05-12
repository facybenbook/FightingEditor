//ノードとノードをつなぐ線の位置（始点、終点）
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ノードの始点か終点か
public enum ConnectionPointType
{
    In,
    Out,
}

public class NodeConnectionPoint
{
    public Rect rect;//位置

    public ConnectionPointType type;//タイプ

    public Node node;//親ノード

    public GUIStyle style;//スタイル

    public Action<NodeConnectionPoint> OnClickConnectionPoint;

    //初期化
    public NodeConnectionPoint(Node node, ConnectionPointType type, GUIStyle style, Action<NodeConnectionPoint> OnClickConnectionPoint)
    {
        this.node = node;//親
        this.type = type;//タイプ
        this.style = style;//スタイル
        this.OnClickConnectionPoint = OnClickConnectionPoint;
        rect = new Rect(0, 0, 10f, 20f);//座標
    }

    //描画
    public void Draw()
    {
        rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;

        //始点か終点か
        switch (type)
        {
            case ConnectionPointType.In:
                rect.x = node.rect.x - rect.width;
                break;

            case ConnectionPointType.Out:
                rect.x = node.rect.x + node.rect.width;
                break;
        }

        //ハンドル矩形が押されたとき線始点終点の登録
        if (GUI.Button(rect, "", style))
        {
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }
    }
}
