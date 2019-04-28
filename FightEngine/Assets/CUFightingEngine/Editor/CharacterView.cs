//
// 17cu0235 村上一真
// Characterビュー管理クラス
//
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using SuperCU;

public class CharacterView : SceneView
{
    //ウィンドウオープン
    [MenuItem("スーパーCU格ゲーエン人17号/キャラクタービュー")]
    public static void Open()
    {
        var window = ScriptableObject.CreateInstance<CharacterView>();
        window.Show();
        window.pivot = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        window.rotation = Quaternion.identity;
        window.SetIconAll(0);
        //ほぼカメラ位置
        window.LookAt(new Vector3(Camera.main.transform.position.x + ConstantEditor.CHARACTER_VIEW_PLACE, Camera.main.transform.position.y, Camera.main.transform.position.z), window.rotation, 1);
        window.name = "CharacterView";
    }
    MethodInfo _internalOnGUI;
    public override void OnEnable()
    {
        base.OnEnable();
        // SceneView.OnGUIを取得する
        var type = typeof(CharacterView);
        _internalOnGUI = type.BaseType.GetMethod("OnGUI", BindingFlags.Instance | BindingFlags.NonPublic);
    }
    //レイヤー制限
    protected override void  OnGUI()
    { 
        if (_internalOnGUI != null)
        {
            int layerMask = 1 << LayerMask.NameToLayer("Character");
            int visibleLayers = Tools.visibleLayers;
            // SceneViewに映るレイヤーを制限してから
            Tools.visibleLayers = layerMask;
            // 標準のSceneView.OnGUIを描く
            base.OnGUI();//下の記述が基底クラスのを呼び出しているだけならこれでもいけそう
            //_internalOnGUI.Invoke(this, null);
            // レイヤーの制限を戻す
            Tools.visibleLayers = visibleLayers;
            
        }
    }
    private void OnLostFocus()
    {
        Tools.visibleLayers = -1;
    }
    private void OnDestroy()
    {
        Tools.visibleLayers = -1;
    }
    //アイコン表示の有無(0で非表示)
    public void SetIconAll(int flag)
    {
        var annotation = Type.GetType("UnityEditor.Annotation, UnityEditor");
        var classId = annotation.GetField("classID");
        var scriptClass = annotation.GetField("scriptClass");

        var annotationUtility = Type.GetType("UnityEditor.AnnotationUtility, UnityEditor");
        var getAnnotations = annotationUtility.GetMethod("GetAnnotations", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
        var setIconEnabled = annotationUtility.GetMethod("SetIconEnabled", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

        var annotations = getAnnotations.Invoke(null, null) as Array;

        foreach (var n in annotations)
        {
            var parameters = new object[]
            {
                ( int )classId.GetValue( n ),
                ( string )scriptClass.GetValue( n ),
                flag,
            };

            setIconEnabled.Invoke(null, parameters);
        }
    }

    //private void Update()
    //{
    //    if (mouseOverWindow != null)
    //    {
    //        if (mouseOverWindow.name == base.name)
    //        {
    //            Drag();
    //        }
    //    }
    //}
    //public void Drag()
    //{
    //    DragAndDrop.PrepareStartDrag();
    //}
    
}
