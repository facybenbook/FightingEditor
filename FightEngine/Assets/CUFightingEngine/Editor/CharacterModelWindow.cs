using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public partial class CharacterModelWindow : ScriptableWizard
{
    public static CharacterModelWindow window;

    private GameObject modelObj;
    private string charaName;
    [MenuItem("格ゲーエンジン/キャラ作成")]
    static void Open()
    {
        if (window == null)
        {
            window = DisplayWizard<CharacterModelWindow>("CharacterModelWindow");
        }
        window.createButtonName = "キャラクター作成";     
    }
    //ScriptableWizard用のonGUI(onGUIは内部で既に使われているため)
    protected override bool DrawWizardGUI()
    {

        base.DrawWizardGUI();
        modelObj = EditorGUILayout.ObjectField("キャラクターモデル",modelObj,typeof(GameObject),false)as GameObject;
        charaName = EditorGUILayout.TextField("キャラ名",  charaName);
        
        return true;
    }
    //右下のボタンを押された時の判定
    private void OnWizardCreate()
    {
        
        GameObject obj = PrefabUtility.InstantiatePrefab(modelObj) as GameObject;
        AssetDatabase.CreateFolder("Assets/Character", charaName);
        PrefabUtility.SaveAsPrefabAsset(obj, "Assets/Character/" + charaName +"/"+charaName+ ".prefab");
        CreateParam();
        CharacterView.Open();
        CreateScript();
        Close();
    }
    //パラメータ(ScriptableObjectAssetの作成)
    private CharacterProperty CreateParam()
    {
        AssetDatabase.CreateFolder("Assets/Character/" + charaName, "Paramater");
        return CharacterProperty.Create("Assets/Character/" + charaName + "/Paramater/" + charaName + ".asset");
    }
    private void CreateScript()
    {
        AssetDatabase.CreateFolder("Assets/Character/" + charaName, "Script");
        string path = "Assets/Character/" + charaName + "/Script/" + charaName + ".cs";
        string classtemp = CodeTemp();
        classtemp = classtemp.Replace("#CLASS_NAME#", charaName);
        File.WriteAllText(path, classtemp);
        AssetDatabase.Refresh();
    }
}
