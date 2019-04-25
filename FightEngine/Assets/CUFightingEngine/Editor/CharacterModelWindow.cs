using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterModelWindow : ScriptableWizard
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
        PrefabUtility.SaveAsPrefabAsset(obj, "Assets/" + charaName + ".prefab");
        CharacterView.Open();
        Close();
    }

}
