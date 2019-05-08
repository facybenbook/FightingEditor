#if UNITY_EDITOR
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
using SuperCU.Generic;
using System.Reflection;

namespace SuperCU.FightingEngine
{
    [System.Serializable]
    public partial class CharacterModelWindow : EditorWindow
    {
        public static CharacterModelWindow window; //ウィンドウ

        //入力用
        private GameObject modelObj; //モデル（ゲームオブジェクト）
        private string charaName; //名前(半角英数)

        private GameObject tempObject = null;

        //ウィンドウ表示
        [MenuItem("スーパーCU格ゲーエン人17号/キャラ作成")]
        static void Open()
        {
            if (window == null)
            {
                window = CreateInstance<CharacterModelWindow>();
            }
            window.Show();
        }

        //GUI表示
        private void OnGUI()
        {
            //プレハブにアタッチして保存
            if (tempObject != null)
            {
                if (SubType.GetType(charaName)!=null)
                {
                    tempObject.AddComponent(SubType.GetType(charaName));
                    //プレハブのセーブ
                    PrefabUtility.SaveAsPrefabAsset(tempObject, "Assets/Character/" + charaName + "/" + charaName + ".prefab");
                    tempObject = null;
                    Close();
                }
            }
            modelObj = EditorGUILayout.ObjectField("キャラクターモデル", modelObj, typeof(GameObject), false) as GameObject; //オブジェクト入力欄の表示
            charaName = EditorGUILayout.TextField("キャラ名(半角英字)", charaName); //テキスト入力欄の表示
            if (charaName != null)
            {
                if (charaName.Length > 0)
                {
                    charaName = SubString.OneUpper(charaName); //1文字目を大文字に変換
                }
            }
            //ボタンが押されたときの処理
            if (GUILayout.Button("キャラクタ生成"))
            {
                if (!Directory.Exists("Assets/Character"))
                {
                    AssetDatabase.CreateFolder("Assets", "Character");
                }
                CreateScript();
                CharacterView.Open();//CharacterView
                Scene scene = EditorSceneManager.OpenScene(ConstantsEditor.CHARACTER_VIEW_SCENE_PATH, OpenSceneMode.Additive);//CharacterView用のシーンをロード
                modelObj = CreatePrefab(scene);//プレハブ、オブジェクト作成
                CreateParam();//パラメータ作成
                SceneManager.SetActiveScene(scene);//アクティブシーンの変更
            }
        }
        //スクリプトテンプレートの作成（テンプレートは別csファイル）
        private void CreateScript()
        {
            AssetDatabase.CreateFolder("Assets/Character", charaName);//フォルダ作成
            AssetDatabase.CreateFolder("Assets/Character/" + charaName, "Script");//フォルダ作成
            string path = "Assets/Character/" + charaName + "/Script/" + charaName + ".cs";
            string classtemp = CodeTempCharacter();
            //テンプレート内の特定文字列の変換
            classtemp = classtemp.Replace("#CLASS_NAME#", charaName);
            File.WriteAllText(path, classtemp);
            AssetDatabase.Refresh();
        }
        //パラメータ(ScriptableObjectAssetの作成)
        private CharacterProperty CreateParam()
        {
            AssetDatabase.CreateFolder("Assets/Character/" + charaName, "Paramater");
            return CharacterProperty.Create("Assets/Character/" + charaName + "/Paramater/" + charaName + ".asset");
        }
        //プレハブ作成
        private GameObject CreatePrefab(Scene scene)
        {
            //キャラクターの親オブジェ登録
            GameObject parantCharacter = (GameObject)Resources.Load(ConstantsEditor.CHARACTER_PARANT_RESOURCES_PATH);
            parantCharacter = PrefabUtility.InstantiatePrefab(parantCharacter, scene) as GameObject;
            GameObject obj = PrefabUtility.InstantiatePrefab(modelObj, scene) as GameObject;
            obj.transform.parent = parantCharacter.transform;
            obj.transform.localPosition = Vector3.zero;
            parantCharacter.name = charaName;
            tempObject = parantCharacter;
            //子オブジェのレイヤーを変更
            List<Transform> children = SubTransform.ChildClass(parantCharacter.transform);//子オブジェを全て読みこみ
            foreach (Transform child in children)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Character");
            }
            parantCharacter.layer = LayerMask.NameToLayer("Character");

            //オブジェクトの位置移動
            parantCharacter.transform.position = new Vector3(ConstantsEditor.CHARACTER_VIEW_PLACE, 0, 0);

            CharacterView.characterPrefab = parantCharacter;

            //プレハブのセーブ
            return PrefabUtility.SaveAsPrefabAsset(parantCharacter, "Assets/Character/" + charaName + "/" + charaName + ".prefab");
        }
        private Type GetTypeByClassName(string className)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Name == className)
                    {
                        return type;
                    }
                }
            }
            return null;
        }
    }
}
#endif
