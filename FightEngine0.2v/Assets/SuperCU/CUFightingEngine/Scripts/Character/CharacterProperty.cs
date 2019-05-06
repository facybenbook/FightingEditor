using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperCU.FightingEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CharacterProperty : ScriptableObject
{
#if UNITY_EDITOR
    public static CharacterProperty Create(string folder)
    {
        // ScriptableObject.CreateInstance()でインスタンスを生成
        // この時点ではアセット化はされていない
        var asset = CreateInstance<CharacterProperty>();
        // アセット化するにはAssetDatabase.CreateAsset()
        // 拡張子は必ず.assetとする
        GameProperty game = Resources.Load(ConstantsEditor.GAME_PROPERTY_RESOURCES_PATH) as GameProperty;
        game.characterProperties.Add(asset);
        AssetDatabase.SaveAssets();
        AssetDatabase.CreateAsset(asset, folder);
        AssetDatabase.Refresh();
        return asset;
    }
#endif
    [SerializeField]
    private int id;
    [SerializeField]
    private int initHp;
}
