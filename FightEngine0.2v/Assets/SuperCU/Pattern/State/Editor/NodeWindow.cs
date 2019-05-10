using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using SuperCU.Pattern;
using System.Reflection;
using System;
using UnityEngine.Events;
using UnityEditor.Events;

public partial class Node
{
    private Component[] compornents; //取得するコンポーネント
    private List<MethodInfo> nowMethods; //現在のメソッド集
    private string[] nowOptions; //現在のメソッド名集
    private int nowStringNumber; //現在の番号

    //GUIウィンドウ用関数
    void DrawNodeWindow(int id)
    {
		//自身のウィンドウをアクティブにする
		if (Event.current.button == 0 && Event.current.type == EventType.MouseDown)
        {
            nowActivId = id;
        }
        //入力モード選択
        mode = (Mode)EditorGUILayout.EnumPopup(mode);
        //ドラッグ可能
        GUI.DragWindow(new Rect(0, 0, 200, 20));
		//入力
		if (nowActivId == id)
        {
            switch (mode)
            {
                case Mode.FUNC:
                    bool compornentsFlag = false;
                    window.height = 115;//windowの高さ変更
                    //ラベル表示
                    EditorGUILayout.LabelField("State移行時に呼び出すもの");
                    EditorGUILayout.LabelField("アタッチされているスクリプトから");
                    EditorGUILayout.LabelField("引数のないpublicメソッドのみ");
                    //アタッチされているすべてのコンポーネントを取得(コンポーネントが違えば取得)
                    if (compornents != gameObject.GetComponents<Component>())
                    {
                        compornents = gameObject.GetComponents<Component>();
                        compornentsFlag = true;
                    }
                    //コンポーネントが変更されたときの処理
                    if (compornentsFlag)
                    {
                        //取得するメソッドの条件
                        var atr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly;
                        List<MethodInfo> methods = new List<MethodInfo>();//取得するメソッドリスト
                        //コンポーネントごとに回して格納(transform,Animatorは含まない)
                        for (int i = 1; i < compornents.Length; i++)
                        {
                            if (compornents[i].GetType() == typeof(Animator))
                            {
                                continue;
                            }
                            Type type = compornents[i].GetType();//コンポーネントのタイプの取得
                            methods.AddRange(type.GetMethods(atr));//メソッドに追加
                        }
                        //引数のあるメソッドを削除
                        for (int j = 0; j < methods.Count; j++)
                        {
                            if (methods[j].GetParameters().Length > 0)
                            {
                                methods.Remove(methods[j]);
                            }
                        }
                        //使えるメソッドがあれば名前取得
                        string[] options = { };
						int count = 0;
						if (methods.Count > 0)
                        {
                            foreach (MethodInfo method in methods)
                            {
                                Array.Resize(ref options, options.Length + 1);
                                options[count] = method.Name;
                                count++;
                            }
                        }
						Array.Resize(ref options, options.Length + 1);
						//最後にNoneを追加
						options[count] = "None";
						nowMethods = methods;
                        nowOptions = options;
                        compornentsFlag = false;//一応
                    }
                    //メソッド選択
                    state.stringNumber = EditorGUILayout.Popup(state.stringNumber, nowOptions);
                    //メソッドがあれば
                    if (nowStringNumber != state.stringNumber)
                    {
                        nowStringNumber = state.stringNumber;

						////タイプからインスタンスを作成する方法
						//var args = new object[] { };
						//var bar = Activator.CreateInstance(typeClone, args);

						//CreateDelegateは第二引数にインスタンス
						//デリゲートを作成し、変数に入れる
						if (state.stringNumber >= nowMethods.Count)
						{
							for (int i = 0; i < state.playDelegate.GetPersistentEventCount() + 1; i++)
							{
								UnityEventTools.RemovePersistentListener(state.playDelegate, i);
							}
							break;
						}
						Type t = nowMethods[state.stringNumber].DeclaringType;
						if (state.playDelegate.GetPersistentEventCount() != 0)
						{

							for (int i = 0; i < state.playDelegate.GetPersistentEventCount() + 1; i++)
							{
								UnityEventTools.RemovePersistentListener(state.playDelegate, i);
							}
						}
						UnityEventTools.AddVoidPersistentListener(state.playDelegate, (UnityAction)nowMethods[state.stringNumber].CreateDelegate(typeof(UnityAction), gameObject.transform.GetComponent(t)));
						
                    }
                    break;
            }
        }
        else
        {
            window.height = 50;
        }
    }
}
