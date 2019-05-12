//描画ノードクラス
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Node
{
    public Rect rect; //ノードの位置
    public string title;//ノードの名前

    public bool isDragged;//ドラッグ中かどうか
    public bool isSelect;//選択中かどうか

    public NodeConnectionPoint inPoint;//ノードの入ってくるポイント
    public NodeConnectionPoint outPoint;//ノードの出るポイント

    public GUIStyle style;//現在のスタイル
    public GUIStyle defaultNodeStyle;//通常のスタイル
    public GUIStyle selectedNodeStyle;//選択中のスタイル

    public Action<Node> OnRemoveNode;//ノード削除
    public Rect window;//このノードの大きさ


    //初期化
    public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<NodeConnectionPoint> OnClickInPoint, Action<NodeConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
    {
        rect = new Rect(position.x, position.y, width, height);
        inPoint = new NodeConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoint = new NodeConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);
        style = nodeStyle;
        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;
    }

    //ドラッグ
    public void Drag(Vector2 vec2)
    {
        rect.position += vec2;
    }

    //自身を描画する
    public void Draw()
    {
        inPoint.Draw();
        outPoint.Draw();
        //GUI.Box(rect, title, style);//矩形ベースで描画
        rect = GUI.Window(55245, rect, WindowFunc,"sss");
    }
    private void WindowFunc(int id)
    {
        EditorGUILayout.LabelField("manm");
    }

    //イベントごとの挙動
    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            //マウスが押されたとき
            case EventType.MouseDown:
                //左クリック
                if (e.button == 0)
                {
                    //ノード内にマウスがあるかどうか
                    if (rect.Contains(e.mousePosition))
                    {
                        //変更があったことを知らせる
                        isDragged = true;
                        GUI.changed = true;
                        //選択中にしてスタイルを変える
                        isSelect = true;
                        style = selectedNodeStyle;
                    }
                    else
                    {
                        GUI.changed = true;
                        //選択解除
                        isSelect = false;
                        style = defaultNodeStyle;
                    }
                }
                //選択中で右クリック
                if (e.button == 1 && isSelect && rect.Contains(e.mousePosition))
                {
                    //ノード削除のコンテキストメニュー追加
                    ProcessContextMenu();
                    e.Use();
                }

                break;

            //マウスを離した時
            case EventType.MouseUp:
                //ドラッグ中止
                isDragged = false;
                break;

            //ドラッグ中
            case EventType.MouseDrag:
                //左クリック
                if (e.button == 0 && isDragged)
                {
                    //ドラッグ処理
                    Drag(e.delta);
                    //他のイベント処理を抑制
                    e.Use();
                    return true;
                }
                break;
        }
        return false;
    }
    //ノード削除のコンテキストメニュー
    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }
    //ノード削除
    private void OnClickRemoveNode()
    {
        if (OnRemoveNode != null)
        {
            OnRemoveNode(this);
        }
    }
}
