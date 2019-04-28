//
// 17CU0235 村上一真
// エディタ用定数置き場
//

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperCU
{
    public static class ConstantEditor
    {
        public const float CHARACTER_VIEW_PLACE = -300; //キャラクタビューの場所（Ｘ軸）
        public const string CHARACTER_VIEW_SCENE_PATH = "Assets/CUFightingEngine/Scenes/CharacterView.unity";//キャラクタビューのシーンパス
        public const string CHARACTER_PARANT_PATH = "SuperCU/Character/CharacterParant";//キャラクタの親オブジェクトのパス
    }
}
#endif