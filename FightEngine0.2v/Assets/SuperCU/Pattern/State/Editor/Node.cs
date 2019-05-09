using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using SuperCU.Pattern;
using System.Reflection;
using System;
//ベースとなるノードクラス
public partial class Node
{
    public static int nowActivId;//現在アクティブのノード
    public enum Mode
    {
        FUNC,//State移行時に呼び出される関数
        ANIMATION,//再生するアニメーション
    }
    private Mode mode; //現在入力しているモード
    public string name; //このノードの名前
    public int id; //このノードのＩＤ
    public Rect window;//このノードの大きさ
    public List<Node> childs = new List<Node>();//このノードの移行先
    public Color color = Color.white;//このノードのカラー
    public StateString state;//このノードのステート情報
    public GameObject gameObject;//このノードに使用するスクリプトのアタッチされているゲームオブジェクト

    //初期化
    public Node(string na, Vector2 position, int d, StateString ss, GameObject game)
    {
        gameObject = game;
        state = ss;
        id = d;
        name = na;
        window = new Rect(position, new Vector2(200, 50));
    }
    //描画
    public void Draw()
    {
        //色変え
        GUI.backgroundColor = color;
        //ノードウィンドウ作成
        window = GUI.Window(id, window, DrawNodeWindow, name + id);
        //ノード同士をつなぐ線
        foreach (var child in childs)
        {
            DrawNodeLine(this.window, child.window);
        }
    }
    //線の描画
    static void DrawNodeLine(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);
        Vector3 harhPos = (startPos + endPos) / 2;
        Vector3 direction = (endPos - startPos).normalized;
        //色は適当
        Handles.color = Color.white;
        Handles.DrawLine(startPos, endPos);
        Handles.color = Color.white;
        //三角形ポリゴン（矢印）作成
        Handles.DrawAAConvexPolygon(
            harhPos + (new Vector3(-direction.y, direction.x, 0).normalized * 6) + new Vector3(0, 0, -5),
            harhPos + (new Vector3(-direction.y, direction.x, 0).normalized * -6) + new Vector3(0, 0, -5),
            harhPos + (direction * 10) + new Vector3(0, 0, -5));
    }

}
//public static int nowId;
////enum Mode
////{
////    FUNC,//ステート移行時に呼び出されるメソッド
////}
//private Mode mode;
//public int id;
//public string name;
//public Rect window;
//public List<Node> childs = new List<Node>();
//public Color color = Color.white;
//public StateString state;//どのステートか
//GameObject gameObject;
//MonoScript mono;//現在のスクリプト
//int stringNumber;//現在の番号
//string[] optionsClone = { };//現在表示中のメソッド名
//List<MethodInfo> methodsClone = new List<MethodInfo>();
//Type typeClone;
//Component[] compornents;
////初期化
//public Node(string na, Vector2 position, int d, StateString ss, GameObject game)
//{
//    gameObject = game;
//    state = ss;
//    id = d;
//    name = na;
//    window = new Rect(position, new Vector2(200, 50));
//}
////描画
//public void Draw()
//{
//    //色変え
//    GUI.backgroundColor = color;
//    window = GUI.Window(id, window, DrawNodeWindow, name + id);
//    foreach (var child in childs)
//    {
//        DrawNodeLine(this.window, child.window);
//    }
//}
////GUIウィンドウ用関数
//void DrawNodeWindow(int id)
//{

//    //どのウィンドウがアクティブか
//    if (Event.current.button == 0 && Event.current.type == EventType.MouseDown)
//    {
//        nowId = id;
//    }
//    mode = (Mode)EditorGUILayout.EnumPopup(mode);
//    GUI.DragWindow(new Rect(0, 0, 200, 20));

//    //中身の入力
//    if (nowId == id)
//    {
//        //ステート移行時に呼び出されるメソッド入力欄
//        if (mode == Mode.FUNC)
//        {
//            window.height = 115;//windowの高さ変更
//                                //ラベル
//            EditorGUILayout.LabelField("State移行時に呼び出すもの");
//            EditorGUILayout.LabelField("引数のないpublicメソッドのみ");
//            compornents = gameObject.GetComponents<Component>();
//            //state.playScript = EditorGUILayout.ObjectField(state.playScript, typeof(MonoScript), true) as MonoScript;//スクリプトアタッチ
//            List<MethodInfo> methods;
//            if (state.playScript != null)
//            {
//                ////スクリプトが入れ替わった時だけ
//                //if (mono != state.playScript)
//                //{
//                mono = state.playScript;
//                var atr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly;
//                Type type = state.playScript.GetClass();
//                /*メソッドの初期化*/
//                methods = new List<MethodInfo>();
//                methods.AddRange(type.GetMethods(atr));
//                string[] options = { };
//                int i = 0;
//                //引数のないメソッドだけ入れる
//                for (int j = 0; j < methods.Count; j++)
//                {
//                    if (methods[j].GetParameters().Length > 0)
//                    {
//                        methods.Remove(methods[j]);
//                    }
//                }
//                if (methods.Count > 0)
//                {
//                    //メソッドの名前取得
//                    foreach (MethodInfo method in methods)
//                    {
//                        Array.Resize(ref options, options.Length + 1);
//                        options[i] = method.Name;
//                        i++;
//                    }
//                }
//                optionsClone = options;
//                methodsClone = methods;
//                typeClone = type;
//                //}
//                state.stringNumber = EditorGUILayout.Popup(state.stringNumber, optionsClone);
//                if (methodsClone.Count > 0)
//                {
//                    if (stringNumber != state.stringNumber)
//                    {
//                        stringNumber = state.stringNumber;
//                        //タイプからインスタンスを作成（CreateDelegateは第二引数にインスタンスを渡すため）
//                        var args = new object[] { };
//                        var bar = Activator.CreateInstance(typeClone, args);
//                        //デリゲートを作成し、変数に入れる
//                        state.playDelegate = (StateBase.playState)methodsClone[state.stringNumber].CreateDelegate(typeof(StateBase.playState), GameManager.Instance);
//                    }
//                }
//            }
//        }
//    }
//    else
//    {
//        window.height = 50;
//    }
//}
//static void DrawNodeLine(Rect start, Rect end)
//{
//    Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height / 2, 0);
//    Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);
//    Vector3 harhPos = (startPos + endPos) / 2;
//    Vector3 direction = (endPos - startPos).normalized;
//    //色は適当
//    Handles.color = Color.white;
//    Handles.DrawLine(startPos, endPos);
//    Handles.color = Color.white;
//    //三角形ポリゴン（矢印）作成
//    Handles.DrawAAConvexPolygon(
//        harhPos + (new Vector3(-direction.y, direction.x, 0).normalized * 6) + new Vector3(0, 0, -5),
//        harhPos + (new Vector3(-direction.y, direction.x, 0).normalized * -6) + new Vector3(0, 0, -5),
//        harhPos + (direction * 10) + new Vector3(0, 0, -5));
//}
