//
// 17CU0235 村上一真
// CharacterModelWindow.cs
// キャラクタモデルと名前を入力してプレハブとスクリプトテンプレートを作成するウィンドウクラス
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using UnityEditor.Callbacks;
using System;
using SuperCU;

public partial class CharacterModelWindow : ScriptableWizard
{
    public static CharacterModelWindow window;//ウィンドウは一つ

    private GameObject modelObj;　//モデル（ゲームオブジェクト）
    private string charaName; //名前(半角英数)
    private string charaNameJp;   //名前(日本語)

    //ウィンドウ表示
    [MenuItem("スーパーCU格ゲーエン人17号/キャラ作成")]
    static void Open()
    {
        if (window == null)
        {
            window = DisplayWizard<CharacterModelWindow>("CharacterModelWindow");
        }
        window.createButtonName = "キャラクター作成";     
    }

    //ScriptableWizard用のonGUI(onGUIは内部で既に使われているため)
    //表示を変更
    protected override bool DrawWizardGUI()
    {
        base.DrawWizardGUI();//念のため
        modelObj = EditorGUILayout.ObjectField("キャラクターモデル",modelObj,typeof(GameObject),false)as GameObject;
        charaName = EditorGUILayout.TextField("キャラ名(半角英字)",  charaName);
        charaNameJp = EditorGUILayout.TextField("キャラ名(全角ＯＫ)", charaNameJp);
        return true;
    }
    //右下のボタンを押された時の判定
    private void OnWizardCreate()
    {　
        AssetDatabase.CreateFolder("Assets/Character", charaName);//フォルダの作成
        CreateScript();//スクリプトテンプレートの作成
        CharacterView.charaName = charaName;
        CharacterView.Open();//CharacterView
        Scene scene = EditorSceneManager.OpenScene(ConstantEditor.CHARACTER_VIEW_SCENE_PATH, OpenSceneMode.Additive);//CharacterView用のシーンをロード
        modelObj = CreatePrefab(scene);//プレハブ、オブジェクト作成
        CreateParam();//パラメータ作成
        SceneManager.SetActiveScene(scene);//アクティブシーンの変更
        Close();
    }
    //プレハブ作成
    private GameObject CreatePrefab(Scene scene)
    {
        //キャラクターの親オブジェ登録
        GameObject parantCharacter = (GameObject)Resources.Load(ConstantEditor.CHARACTER_PARANT_PATH);
        parantCharacter = PrefabUtility.InstantiatePrefab(parantCharacter, scene) as GameObject;
        GameObject obj = PrefabUtility.InstantiatePrefab(modelObj,scene) as GameObject;
        obj.transform.parent = parantCharacter.transform;
        obj.transform.localPosition = Vector3.zero;
        parantCharacter.name = charaName;
        parantCharacter.AddComponent ( Type.GetType(charaName) );
        //子オブジェのレイヤーを変更
        List<Transform> children = SuperCU.SubTransform.ChildClass(parantCharacter.transform);//子オブジェを全て読みこみ
        foreach (Transform child in children)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Character");
        }
        parantCharacter.layer = LayerMask.NameToLayer("Character");

        //オブジェクトの位置移動
        parantCharacter.transform.position = new Vector3(ConstantEditor.CHARACTER_VIEW_PLACE, 0, 0);

        CharacterView.characterPrefab = parantCharacter;

        //プレハブのセーブ
        return PrefabUtility.SaveAsPrefabAsset(parantCharacter, "Assets/Character/" + charaName + "/" + charaName + ".prefab");
    }
    //パラメータ(ScriptableObjectAssetの作成)
    private CharacterProperty CreateParam()
    {
        AssetDatabase.CreateFolder("Assets/Character/" + charaName, "Paramater");
        return CharacterProperty.Create("Assets/Character/" + charaName + "/Paramater/" + charaName + ".asset");
    }
    //スクリプトテンプレートの作成（テンプレートは別csファイル）
    private void CreateScript()
    {
        AssetDatabase.CreateFolder("Assets/Character/" + charaName, "Script");
        string path = "Assets/Character/" + charaName + "/Script/" + charaName + ".cs";
        string classtemp = CodeTemp();
        //テンプレート内の特定文字列の変換
        classtemp = classtemp.Replace("#CLASS_NAME#", charaName);
        File.WriteAllText(path, classtemp);
        AssetDatabase.Refresh();
    }
}
