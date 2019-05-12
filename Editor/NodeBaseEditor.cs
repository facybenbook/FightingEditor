//TODO:
//ノード選択が重なった場合に両方とも選択される
//線をつなげた時に二重につなげることが可能
//ウィンドウのID管理

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeBaseEditor : EditorWindow
{
    private List<Node> nodes;    //描画するノード
    private List<NodeConnection> connections;//描画する接続線

    private GUIStyle nodeStyle;//ノードのスタイル
    private GUIStyle selectedNodeStyle;//選択中のノードスタイル
    //ノードの左右矩形ハンドルのスタイル
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;

    //選択中（ドラッグ中）のハンドル
    private NodeConnectionPoint selectedInPoint;
    private NodeConnectionPoint selectedOutPoint;

    private Vector2 offset;//
    private Vector2 drag;//キャンバスのドラッグ


    //window作成
    [MenuItem("SuperCU/NodeWindow")]
    private static void Open()
    {
        NodeBaseEditor window = GetWindow<NodeBaseEditor>();
        window.titleContent = new GUIContent("のーどえでた");
    }

    private void OnEnable()
    {
        ////デフォルトノードスタイルの設定
        //nodeStyle = new GUIStyle();
        //nodeStyle.normal.background = EditorGUIUtility.Load("node0.png") as Texture2D;
        //nodeStyle.border = new RectOffset(20, 12, 12, 12);//大きさ
        //選択中のノードスタイルの設定
        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("node0 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        //接続線用矩形ハンドルスタイルの設定
        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);
        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);

    }

    private void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);//グリッド描画
        DrawGrid(100, 0.4f, Color.gray);//4マスごとに濃く描画

        DrawConnections();//ノード接続線描画
        BeginWindows();
        DrawNodes();//ノード描画
        EndWindows();
        DrawConnectionLine(Event.current);//選択した接続点からマウスの位置までベジェの描画
        //イベント処理
        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);
        //OnGUIを呼び出す（なんらかのコントロールの入力データが変更された場合）
        if (GUI.changed) Repaint();
    }
    private void WindowFunc(int id)
    {
        EditorGUILayout.LabelField("manm");
    }
    //グリッド描画
    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }
    //ノード描画
    private void DrawNodes()
    {
        if(nodes != null)
        {
            foreach(Node node in nodes)
            {
                node.Draw();
            }
        }
    }
    //ノード接続線描画
    private void DrawConnections()
    {
        if (connections != null)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].Draw();
            }
        }
    }
    //選択した接続点からマウスの位置までベジェの描画
    private void DrawConnectionLine(Event e)
    {
        if (selectedInPoint != null && selectedOutPoint == null)
        {
            Handles.DrawBezier(
                selectedInPoint.rect.center,
                e.mousePosition,
                selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null)
        {
            Handles.DrawBezier(
                selectedOutPoint.rect.center,
                e.mousePosition,
                selectedOutPoint.rect.center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }
    }
    //何かイベントが起こった時の挙動
    private void ProcessEvents(Event e)
    {
        drag = Vector2.zero;

        switch (e.type)
        {
            //マウスが押されたとき
            case EventType.MouseDown:
                //右クリックメニューの表示
                if(e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                }
                break;
            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnDrag(e.delta);
                }
                break;
        }
    }
    //何かイベントが起こった時の処理（ノード）
    private void ProcessNodeEvents(Event e)
    {
        if (nodes != null)
        {
            //最後のノードが一番上に描画されるように逆順
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = nodes[i].ProcessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }
    //キャンバス全体のドラッグ処理（ノード全て）
    private void OnDrag(Vector2 delta)
    {
        drag = delta;

        if (nodes != null)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }
    //右クリックメニューの作成
    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        //右クリックメニュー、ノード追加
        genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
        //メニュー表示
        genericMenu.ShowAsContext();
    }
    //ノードの追加
    private void OnClickAddNode(Vector2 mousePosition)
    {
        if (nodes == null)
        {
            nodes = new List<Node>();
        }
        //マウスの場所に追加
        nodes.Add(new Node(mousePosition, 200, 50, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
    }

    //始点が押されたとき登録
    private void OnClickInPoint(NodeConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }
    //終点が押されたとき登録
    private void OnClickOutPoint(NodeConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;

        if (selectedInPoint != null)
        {
            //現在選択中の始点終点ノードがあればノード線を作成
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }
    //真ん中の矩形
    private void OnClickRemoveConnection(NodeConnection connection)
    {
        connections.Remove(connection);
    }
    //ノード線の作成
    private void CreateConnection()
    {
        if (connections == null)
        {
            connections = new List<NodeConnection>();
        }

        connections.Add(new NodeConnection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
    }
    //選択中のノード矩形のリセット
    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }
    private void OnClickRemoveNode(Node node)
    {
        if (connections != null)
        {
            List<NodeConnection> connectionsToRemove = new List<NodeConnection>();
            //他ノードへの接続線の削除
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint)
                {
                    connectionsToRemove.Add(connections[i]);
                }
            }
            for (int i = 0; i < connectionsToRemove.Count; i++)
            {
                connections.Remove(connectionsToRemove[i]);
            }

            connectionsToRemove = null;
        }
        //ノードの削除
        nodes.Remove(node);
    }
}
