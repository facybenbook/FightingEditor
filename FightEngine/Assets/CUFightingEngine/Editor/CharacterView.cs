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
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using SuperCU;

public partial class CharacterView : SceneView
{
    public static string charaName = null;
    public static GameObject characterPrefab = null;
    //public static CharacterViewWindow inputWindow;//入力フォーム
    public static GameObject CharacterPrefab //シーン内削除して代入
    {
        get { return characterPrefab; }
        set
        {
            Scene scene = SceneManager.GetSceneByName("CharacterView");
            SceneManager.SetActiveScene(scene);//アクティブシーンの変更
            if (value != null)
            {
                if (characterPrefab == null)
                {
                    GameObject[] objects = scene.GetRootGameObjects();
                    if (objects.Length > 0)
                    {
                        foreach (GameObject obj in objects)
                        {
                            GameObject.DestroyImmediate(obj);
                        }
                    }
                }
                else if (value != characterPrefab)
                {
                    GameObject[] objects = scene.GetRootGameObjects();
                    foreach (GameObject obj in objects)
                    {
                        GameObject.DestroyImmediate(obj);
                    }
                }
                characterPrefab = PrefabUtility.InstantiatePrefab(value, scene) as GameObject;
                characterPrefab.transform.position = new Vector3(ConstantEditor.CHARACTER_VIEW_PLACE, 0, 0);

            }
        }
    }

    //ウィンドウオープン
    [MenuItem("スーパーCU格ゲーエン人17号/キャラクタービュー")]
    public static CharacterView Open()
    {
        var window = ScriptableObject.CreateInstance<CharacterView>();
        window.Show();
        window.pivot = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        window.rotation = Quaternion.identity;
        window.SetIconAll(0);
        //ほぼメインカメラ位置に移動
        window.LookAt(new Vector3(Camera.main.transform.position.x + ConstantEditor.CHARACTER_VIEW_PLACE, Camera.main.transform.position.y, Camera.main.transform.position.z), window.rotation, 1);
        window.name = "CharacterView";
        return window;
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
    protected override void OnGUI()
    {

        int layerMask = 1 << LayerMask.NameToLayer("Character");
        int visibleLayers = Tools.visibleLayers;
        // SceneViewに映るレイヤーを制限してから
        Tools.visibleLayers = layerMask;
        // 標準のSceneView.OnGUIを描く
        base.OnGUI();//下の記述が基底クラスのを呼び出しているだけならこれでもいけそう
        GUI.Window(55236845, _windowSize, DrawObjectFieldWindow, "キャラクタ選択");
        //_internalOnGUI.Invoke(this, null);
        // レイヤーの制限を戻す
        Tools.visibleLayers = visibleLayers;
    }
    private void Update()
    {
        
    }
    private void OnLostFocus()
    {
        Tools.visibleLayers = -1;
    }
    private  void OnDestroy()
    {
        base.OnDestroy();
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
}
