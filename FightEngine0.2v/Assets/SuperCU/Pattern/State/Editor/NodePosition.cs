using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperCU.FightingEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class NodePosition : ScriptableObject
{
#if UNITY_EDITOR
	public static CharacterProperty Create(string folder)
	{
		// ScriptableObject.CreateInstance()でインスタンスを生成
		// この時点ではアセット化はされていない
		var asset = CreateInstance<CharacterProperty>();
		// アセット化するにはAssetDatabase.CreateAsset()
		// 拡張子は必ず.assetとする
		AssetDatabase.SaveAssets();
		AssetDatabase.CreateAsset(asset, folder);
		AssetDatabase.Refresh();
		return asset;
	}
#endif
	public List<Rect> positions;
}
