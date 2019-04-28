using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        AssetDatabase.CreateAsset(asset, folder);
        AssetDatabase.Refresh();
        return asset;
    }
#endif
    [SerializeField]
    private int initHp;
}
