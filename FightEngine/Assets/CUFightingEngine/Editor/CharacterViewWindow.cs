//
// 17CU0235 村上一真
// CharacterViewWindow.cs
// CharacterViewの拡張Window記述
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using SuperCU;

[System.Serializable]
public partial class CharacterView : SceneView
{
    [SerializeField]
    private static Rect _windowSize = new Rect(8, 24, 300, 100);
    private static Rect _objFieldSize = new Rect(10, 35, 280, 50);
    private static Rect _pikkerSize = new Rect(10, 10, 100, 20);
    private static GUILayoutOption[] objectLayoutOption = { GUILayout.Width(310), GUILayout.Height(70) };
    //内部ウィンドウ処理
    void DrawObjectFieldWindow(int id)
    {
        //IDの処理をしないとエラーが起きるので注意（デリゲートの場合はなくていい）
        int idW = GUIUtility.GetControlID(FocusType.Passive, _objFieldSize);
        GUIStyle style = EditorStyles.objectFieldThumb;
        style.fontSize = 15;
        style.fontStyle = FontStyle.Bold;
        if (Event.current.type == EventType.Repaint)
        {
            if (CharacterPrefab == null) { style.Draw(_objFieldSize, new GUIContent("  CharacterObject", EditorGUIUtility.IconContent("UnityLogo").image), idW);  }

            else if (CharacterPrefab != null) { style.Draw(_objFieldSize, new GUIContent("  " + CharacterPrefab.name, EditorGUIUtility.IconContent("PreMatCube").image), idW); }
        }

        //ドラッグアンドドロップ処理
        if (_objFieldSize.Contains(Event.current.mousePosition))
        {
            switch (Event.current.type)
            {
                //ドラッグ終了 = ドロップ
                case EventType.DragExited:
                    DragAndDrop.activeControlID = idW;
                    //ドロップしているのが参照可能なオブジェクトの場合
                    if (DragAndDrop.objectReferences.Length == 1)
                    {
                        var reference = DragAndDrop.objectReferences[0] as GameObject;
                        if (reference != null)
                        {
                            CharacterPrefab = reference;
                            HandleUtility.Repaint();
                            //ここでEv.Use()するとその後の処理が出来ずヒエラルキーに表示されないオブジェクトが作成されたので注意
                        }
                        HandleUtility.Repaint();
                    }
                    break;
                //ドラッグ中
                case EventType.DragUpdated:
                case EventType.DragPerform:

                    //ドラッグしているのが参照可能なオブジェクトの場合
                    if (DragAndDrop.objectReferences.Length == 1)

                        //オブジェクトを受け入れる
                        DragAndDrop.AcceptDrag();
                    //ドラッグしているものを現在のコントロール ID と紐付ける
                    DragAndDrop.activeControlID = idW;
                    //カーソルの見た目を変える
                    DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                    Event.current.Use();//
                    break;
            }
        }
    }
}
